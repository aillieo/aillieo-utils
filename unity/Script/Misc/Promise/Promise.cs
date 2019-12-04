using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AillieoUtils
{
    public class Promise
    {
        public delegate void Callback();

        internal enum State {
            Pending = 0,
            Rejected = 1,
            Resolved = 2,
        }

        State state = State.Pending;
        Queue<Callback> always;
        Queue<Callback> dones;
        Queue<Callback> fails;

        public Promise()
        {
            this.state = State.Pending;
        }

        public Promise Done(Callback func)
        {
            if (state == State.Pending)
            {
                this.dones = this.dones ?? new Queue<Callback>();
                this.dones.Enqueue(func);
            }
            else
            {
                func();
            }
            return this;
        }

        public Promise Fail(Callback func)
        {
            if (state == State.Pending)
            {
                this.fails = this.fails ?? new Queue<Callback>();
                this.fails.Enqueue(func);
            }
            else
            {
                func();
            }
            return this;
        }

        public Promise Always(Callback func)
        {
            if(state == State.Pending)
            {
                this.always = this.always ?? new Queue<Callback>();
                this.always.Enqueue(func);
            }
            else
            {
                func();
            }
            return this;
        }

        public void Resolve()
        {
            if (state != State.Pending)
            {
                throw new Exception("Resolve when state is " + state);
            }

            state = State.Resolved;

            if(always != null)
            {
                while (always.Count > 0)
                {
                    always.Dequeue().Invoke();
                }
                always = null;
            }

            if(dones != null)
            {
                while (dones.Count > 0)
                {
                    dones.Dequeue().Invoke();
                }
                dones = null;
            }

            fails.Clear();
            fails = null;
        }

        public void Reject()
        {
            if(state != State.Pending)
            {
                throw new Exception("Reject when state is " + state);
            }

            state = State.Rejected;

            if(always != null)
            {
                while (always.Count > 0)
                {
                    always.Dequeue().Invoke();
                }
                always = null;
            }

            if(fails != null)
            {
                while (fails.Count > 0)
                {
                    fails.Dequeue().Invoke();
                }
                fails = null;
            }

            dones.Clear();
            dones = null;
        }

        public static Promise All(params Promise[] promises)
        {
            return All(promises as IList<Promise>);
        }

        public static Promise All(IList<Promise> promises)
        {
            Promise promise = new Promise();
            int count = promises.Count;
            if (count == 0)
            {
                promise.Resolve();
                return promise;
            }

            int rest = count;
            bool success = true;

            for (int i = 0; i < count; ++i)
            {
                promises[i].Done(() =>
                {
                    rest--;
                    if (rest == 0)
                    {
                        if (success)
                        {
                            promise.Resolve();
                        }
                        else
                        {
                            promise.Reject();
                        }
                    }
                })
                .Fail(() =>
                {
                    rest--;
                    success = false;
                    if (rest == 0)
                    {
                        promise.Reject();
                    }
                });
            }

            return promise;
        }

    }
}
