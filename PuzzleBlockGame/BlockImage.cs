using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleBlockGame
{
    public class BlockImage
    {
        private Bitmap[] images;
        public BlockImage()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            images = new Bitmap[5];
            Stream stream = assembly.GetManifestResourceStream("PuzzleBlockGame.Image.Block1.png");
            images[0] = new Bitmap(stream);
            stream = assembly.GetManifestResourceStream("PuzzleBlockGame.Image.Block2.png");
            images[1] = new Bitmap(stream);
            stream = assembly.GetManifestResourceStream("PuzzleBlockGame.Image.Block3.png");
            images[2] = new Bitmap(stream);
            stream = assembly.GetManifestResourceStream("PuzzleBlockGame.Image.Block4.png");
            images[3] = new Bitmap(stream);
            stream = assembly.GetManifestResourceStream("PuzzleBlockGame.Image.Block5.png");
            images[4] = new Bitmap(stream);

        }

        public Image this[BlockType blockType]
        {
            get
            {
                if (blockType == BlockType.None)
                {
                    return null;
                }
                else
                {
                    return images[(int)blockType];
                }
            }
        }

    }
}
