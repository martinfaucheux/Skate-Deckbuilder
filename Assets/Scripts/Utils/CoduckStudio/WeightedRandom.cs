using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoduckStudio.Utils
{
    public class WeightedRandom
    {
        [Serializable] public class Weight<T>: ICloneable
        {
            public int weight = 1;
            public T data = default(T);

            public object Clone()
            {
                return new Weight<T>
                {
                    weight = this.weight,
                    data = this.data
                }; 
            }
        }

        public static T GetRandom<T>(List<Weight<T>> list, System.Random rng)
        {
            int allWeights = 0;
            foreach (Weight<T> weight in list) {
                allWeights += weight.weight;
            }


            T ret = default(T);
            
            // string listAsString = String.Join(", ", list.Select((v) => v.weight.ToString()));
            // Debug.Log($"WeightedRandom::GetRandom(): START allWeights={allWeights} list=[{listAsString}]");

            int randomNumber = rng.Next(1, allWeights + 1);
            int lastWeightIterator = 1;
            int i = 0;
            for (i = 0; i < list.Count; i++) {
                // Debug.Log($"WeightedRandom::GetRandom(): ONGOING index={i} lastWeightIterator={lastWeightIterator} randomNumber>=lastWeightIterator={randomNumber >= lastWeightIterator} randomNumber<=lastWeightIterator+list[i].weight={randomNumber <= lastWeightIterator + list[i].weight}");
                if (randomNumber >= lastWeightIterator && randomNumber <= lastWeightIterator + list[i].weight) {
                    ret = list[i].data;
                    break;
                }
                
                lastWeightIterator += list[i].weight;
            }

            // Debug.Log($"WeightedRandom::GetRandom(): END randomNumber={randomNumber} allWeights={allWeights} indexTaken={i} list=[{listAsString}]");

            return ret;
        }

        public static List<T> GetRandoms<T>(List<Weight<T>> list, int count, System.Random rng)
        {
            List<Weight<T>> duplicatedList = new();
            List<T> ret = new();

            int allWeights = 0;
            foreach (Weight<T> weight in list) {
                if (weight.weight == -1) {
                    ret.Add(weight.data);
                }
                else {
                    allWeights += weight.weight;
                    duplicatedList.Add(weight.Clone() as Weight<T>);
                }
            }

            if (ret.Count >= count) {
                return ret.Take(count).ToList();
            }

            do {
                // string listAsString = String.Join(", ", duplicatedList.Select((v) => v.weight.ToString()));
                // Debug.Log($"WeightedRandom::GetRandoms(): START allWeights={allWeights} list=[{listAsString}]");

                int randomNumber = rng.Next(1, allWeights + 1);
                int lastWeightIterator = 1;
                for (int i = 0; i < duplicatedList.Count; i++) {
                    // Debug.Log($"WeightedRandom::GetRandom(): ONGOING index={i} lastWeightIterator={lastWeightIterator} randomNumber>=lastWeightIterator={randomNumber >= lastWeightIterator} randomNumber<=lastWeightIterator+list[i].weight={randomNumber <= lastWeightIterator + list[i].weight}");
                    if (randomNumber >= lastWeightIterator && randomNumber <= lastWeightIterator + duplicatedList[i].weight) {
                        // Debug.Log($"WeightedRandom::GetRandoms(): END randomNumber={randomNumber} allWeights={allWeights} indexTaken={i} list=[{listAsString}]");
                        ret.Add(duplicatedList[i].data);
                        allWeights -= duplicatedList[i].weight;
                        duplicatedList[i].weight = 0;
                        break;
                    }
                    else {
                        lastWeightIterator += duplicatedList[i].weight;
                    }
                }
            } while (ret.Count < count);

            return ret;
        }
    }
}
