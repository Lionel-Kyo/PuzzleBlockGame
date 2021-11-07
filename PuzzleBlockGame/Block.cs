using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleBlockGame
{
    public enum BlockType
    {
        Type1 = 0,
        Type2 = 1,
        Type3 = 2,
        Type4 = 3,
        Type5 = 4,
        None = 5
    }

    public class Block
    {
        private BlockType blockType;
        private PictureBox pictureBox;
        private BlockImage blockImage;
        public Block(BlockType blockType, PictureBox pictureBox, BlockImage blockImage)
        {
            this.pictureBox = pictureBox;
            this.blockImage = blockImage;
            this.BlockType = blockType;
        }
        public BlockType BlockType
        {
            get => blockType;
            set
            {
                blockType = value;
                if (pictureBox != null)
                {
                    //pictureBox.Invoke((MethodInvoker)delegate { this.pictureBox.Image = blockImage[blockType]; });
                    this.pictureBox.Image = blockImage[blockType];
                }
            }
        }
        public PictureBox PictureBox { get => pictureBox; }
    }
}
