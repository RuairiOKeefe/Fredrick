using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.Entity_System
{
	public enum Material
	{
		Empty,
		Dirt,
		Grass
	}
	public enum Shape
	{
		Block,
		UpLeftRamp,
		UpRightRamp,
		DownLeftRamp,
		DownRightRamp
	}

	public enum Facing
	{
		None,
		All,
		Left,
		Right,
		Up,
		Down,
		UpLeft,
		DownLeft,
		DownRight,
		UpRight,
		LeftRight,
		UpDown,
		UpLeftDown,
		LeftDownRight,
		DownRightUp,
		RightUpLeft,
	}

	public class Block
	{
		public Material Material;
		public Shape Shape;
		public int Variant;// variant is defined by the resources, where multiple sprites may be added as a certain block for texture variation

		public Block()
		{
			Material = Material.Empty;
			Shape = Shape.Block;
			Variant = 0;
		}

		public Block(Material material, Shape shape, int variant)
		{
			Material = material;
			Shape = shape;
			Variant = variant;
		}
	}
}
