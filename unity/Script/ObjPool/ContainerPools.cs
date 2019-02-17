using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AillieoUtils
{

    public static class ListPool<T>
    {

        static SimpleObjPool<List<T>> pool = new SimpleObjPool<List<T>>(10,
            (List<T> list) => { list.Clear(); },
            () => { return new List<T>(); });


        public static List<T> Get()
        {
            return pool.Get();
        }


        public static void Recycle(List<T> list)
        {
            pool.Recycle(list);
        }

    }




    public static class DictPool<T,U>
    {

        static SimpleObjPool<Dictionary<T, U>> pool = new SimpleObjPool<Dictionary<T, U>>(10,
            (Dictionary<T, U> dict) => { dict.Clear(); },
            () => { return new Dictionary<T, U>(); });


        public static Dictionary<T, U> Get()
        {
            return pool.Get();
        }


        public static void Recycle(Dictionary<T, U> dict)
        {
            pool.Recycle(dict);
        }

    }

}
