using Fredrick.src.Entity_System;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using BlockResources = Fredrick.src.ResourceManagement.BlockResources;

namespace Fredrick.src.Scenes
{
	public class Level
	{
		public Lighting Lighting { get; private set; }

		readonly private int m_levelWidth;
		readonly private int m_levelHeight;

		private Block[,] m_blockGrid;
		private List<Tuple<string, Vector3>> m_objects;


		public Level(int levelWidth, int levelHeight)
		{
			m_levelWidth = levelWidth;
			m_levelHeight = levelHeight;
			m_blockGrid = new Block[levelWidth, levelHeight];
			m_objects = new List<Tuple<string, Vector3>>();
		}

		//Generates a list of entities from the block grid and returns the list
		public List<Entity> GetBlocks()
		{
			List<Entity> blocks = new List<Entity>();
			for (int i = 0; i < m_levelWidth; i++)
			{
				for (int j = 0; j < m_levelHeight; j++)
				{
					Block block = m_blockGrid[i, j];
					if (block.Material != Material.Empty)
					{
						try
						{
							Entity e = new Entity(BlockResources.Instance.GetBlock(block.Material, block.Shape));
							e.Position = new Vector2((float)i, (float)j);
							blocks.Add(e);
						}
						catch (Exception ex)
						{
							//Log: "Error: Block could not be generated with material: " + block.Material + " shape: " + block.Shape;
						}
					}
				}
			}

			return blocks;
		}


	}
}
