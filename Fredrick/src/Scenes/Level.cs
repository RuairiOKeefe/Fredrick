using Fredrick.src.Entity_System;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using BlockResources = Fredrick.src.ResourceManagement.BlockResources;

namespace Fredrick.src.Scenes
{
	public class Level
	{
		const float blockSize = 0.5f;

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

			Random rnd = new Random();

			for (int i = 0; i < m_levelWidth; i++)
			{
				for (int j = 0; j < m_levelHeight; j++)
				{
					if (i < 4 || i > levelWidth - 4 || j < 4 || j > levelHeight - 4)
					{
						int v = rnd.Next(0, 3);
						m_blockGrid[i, j] = new Block(Material.Dirt, Shape.Block, v);
					}

					if (i > 11 && j == 10 && (i % 8 == 0 || i % 8 == 1 || i % 8 == 2 || i % 8 == 3))
					{
						int v = rnd.Next(0, 3);
						m_blockGrid[i, j] = new Block(Material.Dirt, Shape.Block, v);
					}

					if (i == j && i < 11)
					{
						int v = rnd.Next(0, 3);
						m_blockGrid[i, j] = new Block(Material.Dirt, Shape.Block, v);
					}
				}
			}
		}

		private Facing GetFacing(int i, int j)
		{
			/// . 0 .
			/// 1 x 2 x= CurrentBlock
			/// . 3 .
			/// adjacentBlocks is a string that should always consist of 4 digits, all 0 or 1.
			/// This is a representation of whether or not there is a block on each side of the block.
			/// (A string was used to make this as readable as possible, given the unavoidable ugliness of this solution)
			/// (Yes I'm well aware this entire game is full of ugliness, but I'm one Ru and I want to finish this)
			string adjacentBlocks = "";

			Func<int, int, int> getBlock = (x, y) =>
			{
				if (x < 0 || x > m_levelWidth - 1 || y < 0 || y > m_levelHeight - 1)
				{
					return 1;
				}
				if (m_blockGrid[x, y] == null || m_blockGrid[x, y].Material == Material.Empty)
				{
					return 0;
				}
				else
				{
					return 1;
				}
			};

			adjacentBlocks += getBlock(i, j + 1);
			adjacentBlocks += getBlock(i - 1, j);
			adjacentBlocks += getBlock(i + 1, j);
			adjacentBlocks += getBlock(i, j - 1);


			switch (adjacentBlocks)
			{
				case ("1111"):
					return Facing.None;
				case ("0000"):
					return Facing.All;
				case ("1011"):
					return Facing.Left;
				case ("1101"):
					return Facing.Right;
				case ("0111"):
					return Facing.Up;
				case ("1110"):
					return Facing.Down;
				case ("0011"):
					return Facing.UpLeft;
				case ("1010"):
					return Facing.DownLeft;
				case ("1100"):
					return Facing.DownRight;
				case ("0101"):
					return Facing.UpRight;
				case ("1001"):
					return Facing.LeftRight;
				case ("0110"):
					return Facing.UpDown;
				case ("0010"):
					return Facing.UpLeftDown;
				case ("1000"):
					return Facing.LeftDownRight;
				case ("0100"):
					return Facing.DownRightUp;
				case ("0001"):
					return Facing.RightUpLeft;
				default:
					return Facing.None;
			}
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
					if (block != null)
					{
						if (block.Material != Material.Empty)
						{
							try
							{
								Entity e = new Entity(BlockResources.Instance.GetBlock(block.Material, block.Shape, GetFacing(i, j), block.Variant));
								e.Position = new Vector2((float)i * blockSize, (float)j * blockSize);
								blocks.Add(e);
							}
							catch (Exception ex)
							{
								//Log: "Error: Block could not be generated with material: " + block.Material + " shape: " + block.Shape;
							}
						}
					}
				}
			}

			return blocks;
		}


	}
}
