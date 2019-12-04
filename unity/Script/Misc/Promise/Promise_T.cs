using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static AillieoUtils.Promise;

namespace AillieoUtils
{
    public class Promise<T>
    {
        public delegate void Callback(T arg);

        private T cachedArg;

        State state = State.Pending;
        Queue<Callback> always;
        Queue<Callback> dones;
        Queue<Callback> fails;

        public Promise()
        {
            this.state = State.Pending;
        }

        public Promise<T> Done(Callback func)
        {
            if (state == State.Pending)
            {
                this.dones = this.dones ?? new Queue<Callback>();
                this.dones.Enqueue(func);
            }
            else
            {
                func(cachedArg);
            }
            return this;
        }

        public Promise<T> Fail(Callback func)
        {
            if (state == State.Pending)
            {
                this.fails = this.fails ?? new Queue<Callback>();
                this.fails.Enqueue(func);
            }
            else
            {
                func(cachedArg);
            }
            return this;
        }

        public Promise<T> Always(Callback func)
        {
            if(state == State.Pending)
            {
                this.always = this.always ?? new Queue<Callback>();
                this.always.Enqueue(func);
            }
            else
            {
                func(cachedArg);
            }
            return this;
        }

        public void Resolve(T arg)
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
                    always.Dequeue().Invoke(arg);
                }
                always = null;
            }

            if (dones != null)
            {
                while (dones.Count > 0)
                {
                    dones.Dequeue().Invoke(arg);
                }
                dones = null;
            }

            fails.Clear();
            fails = null;

            cachedArg = arg;
        }

        public void Reject(T arg)
        {
            if(state != State.Pending)
            {
                throw new Exception("Reject when state is " + state);
            }

            state = State.Rejected;

            if (always != null)
            {
                while (always.Count > 0)
                {
                    always.Dequeue().Invoke(arg);
                }
                always = null;
            }

            if(fails != null)
            {
                while (fails.Count > 0)
                {
                    fails.Dequeue().Invoke(arg);
                }
                fails = null;
            }

            dones.Clear();
            dones = null;

            cachedArg = arg;

        }

        public static Promise<IList<T>> All(params Promise<T>[] promises)
        {
            return All(promises as IList<Promise<T>>);
        }

        public static Promise<IList<T>> All(IList<Promise<T>> promises)
        {
            Promise<IList<T>> promise = new Promise<IList<T>>();
            int count = promises.Count;
            if (count == 0)
            {
                promise.Resolve(null);
                return promise;
            }

            IList<T> args = new List<T>(count);

            int rest = count;
            bool success = true;

            for (int i = 0; i < count; ++i)
            {
                promises[i].Done((arg) =>
                {
                    args[i] = arg;
                    rest--;
                    if (rest == 0)
                    {
                        if (success)
                        {
                            promise.Resolve(args);
                        }
                        else
                        {
                            promise.Reject(args);
                        }
                    }
                })
                .Fail((arg) =>
                {
                    args[i] = arg;
                    rest--;
                    success = false;
                    if (rest == 0)
                    {
                        promise.Reject(args);
                    }
                });
            }

            return promise;
        }
    }
}
