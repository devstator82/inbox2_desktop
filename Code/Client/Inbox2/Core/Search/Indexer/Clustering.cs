using System;
using System.Collections.Generic;
using System.Linq;
using m = System.Math;

namespace Inbox2.Core.Search.Indexer
{
	static class Clustering
	{
		public static double Pearson(IEnumerable<long> v1, IEnumerable<long> v2)
		{
			var sum1 = v1.Sum();
			var sum2 = v2.Sum();

			// Sum of the squares
			var sum1Sq = v1.Sum(v => m.Pow(v, 2));
			var sum2Sq = v2.Sum(v => m.Pow(v, 2));

			// Sum of the products
			var l1 = v1.ToArray();
			var l2 = v2.ToArray();

			if (l1.Length != l2.Length)
				throw new ApplicationException("Input data should contain the equal number of items");

			long pSum = Enumerable.Range(0, l1.Length)
				.Select(i => l1[i] * l2[i])
				.Sum();

			// Calculate r (Pearson score)
			var num = pSum - (sum1 * sum2 / l1.Length);
			var den =
				m.Sqrt((sum1Sq - m.Pow(sum1, 2)/l1.Length)*(sum2Sq - m.Pow(sum2, 2)/l2.Length));

			if (den == 0)
				return 0;

			return 1.0 - num /den;
		}

		public static BiCluster HCluster(IEnumerable<WordVector> words)
		{
			var wordsList = words.ToArray();
			var distances = new Dictionary<Pair, double>();
		    int currentClustId = -1;

			List<BiCluster> clust = new List<BiCluster>(wordsList.Length);

			for (int i = 0; i < wordsList.Length; i++)			
				clust.Add(new BiCluster(wordsList[i].Entities.Values, i, 0.0));
			
			while (clust.Count > 1)
			{
				Pair lowestpair = new Pair(0, 1);

				var closest = Pearson(clust[0].Vector, clust[1].Vector);

				// loop through every pair looking for the smallest distance
				for (int i = 0; i < clust.Count; i++)					
				{
					for (int j = i + 1; j < clust.Count; j++)
					{
						// distances is the cache of distance calculations
						if (!distances.ContainsKey(new Pair(clust[i].Id, clust[j].Id)))
						{
							// Calculate the pearson value
							var val = Pearson(clust[i].Vector, clust[j].Vector);
							
							distances.Add(new Pair(clust[i].Id, clust[j].Id), val);
						}

						double d = distances[new Pair(clust[i].Id, clust[j].Id)];

						if (d < closest)
						{
							closest = d;
							lowestpair = new Pair(i, j);
						}
					}
				}

				// calculate the average of the two clusters
                List<long> mergevec = new List<long>();
			    long[] vectorArray = clust[0].Vector.ToArray();
                for (int i = 0; i < vectorArray.Length; i++)
                {
                    long[] leftVectorArray = clust[lowestpair.Left].Vector.ToArray();
                    long[] rightVectorArray = clust[lowestpair.Right].Vector.ToArray();

                    mergevec.Add((leftVectorArray[i] + rightVectorArray[i]) / 2);
                }

                //create the new cluster
                BiCluster newCluster = new BiCluster(mergevec, currentClustId, closest, clust[lowestpair.Left], clust[lowestpair.Right]);
			    
                //cluster ids that weren't in the original set are negative
			    currentClustId--;
			    clust.Remove(clust[lowestpair.Left]);
                clust.Remove(clust[lowestpair.Right]); 
                clust.Add(newCluster);

            }
            return clust[0];
		}
	
		public class BiCluster
		{
			public IEnumerable<long> Vector { get; private set; }
			public int Id { get; private set; }
			public double Distance { get; private set; }
            public BiCluster Left { get; private set; }
            public BiCluster Right { get; private set; }

			public BiCluster(IEnumerable<long> vector, int id, double distance)
			{
				Vector = vector;
				Id = id;
				Distance = distance;
			}

            public BiCluster(IEnumerable<long> vector, int id, double distance, BiCluster left, BiCluster right)
            {
                Vector = vector;
                Id = id;
                Distance = distance;
                Left = left;
                Right = right;
            }
		}

		class Pair
		{
			public int Left { get; set; }

			public int Right { get; set; }

			public Pair(int left, int right)
			{
				Left = left;
				Right = right;
			}

			public override int GetHashCode()
			{
				return String.Format("{0}{1}", Left, Right).GetHashCode();
			}

			public override bool Equals(object obj)
			{
				Pair other = (Pair) obj;

				return String.Format("{0}{1}", Left, Right)
					.Equals(String.Format("{0}{1}", other.Left, other.Right));
			}
		}
	}
}
