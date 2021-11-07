using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleBlockGame
{
    public partial class Main : Form
    {
        private BlockImage blockImage;
        private Random rand;
        private Block[][] gameArr;
        private int stepCount = 0;
        private int secCount = 0;
        private Thread timerThread;

        public Main()
        {
            InitializeComponent();
            blockImage = new BlockImage();
            rand = new Random();
            GameRule_Text.Text = "How to Win:\r\n" +
                "Player need to make sure all 9 blocks in the central panel are same colours by moving the arrows.";
            GameStart();
        }

        private void GameStart()
        {
            gameArr = new Block[9][];
            for (int y = 0; y < 9; y++) 
            {
                gameArr[y] = new Block[9];
            }
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    gameArr[y][x] = new Block(BlockType.None, null, blockImage);
                }
                for (int x = 6; x < 9; x++)
                {
                    gameArr[y][x] = new Block(BlockType.None, null, blockImage);
                }
            }
            for (int y = 6; y < 9; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    gameArr[y][x] = new Block(BlockType.None, null, blockImage);
                }
                for (int x = 6; x < 9; x++)
                {
                    gameArr[y][x] = new Block(BlockType.None, null, blockImage);
                }
            }
            List<int> randList = new List<int>();
            for (int i = 0; i < 45; i++) 
            {
                int randValue = rand.Next(0, 5);
                while (randList.Count(x => x == randValue) == 9)
                {
                    randValue = rand.Next(0, 5);
                }
                randList.Add(randValue);
            }
            gameArr[0][3] = new Block((BlockType)randList[0], upPic1, blockImage);
            gameArr[0][4] = new Block((BlockType)randList[1], upPic2, blockImage);
            gameArr[0][5] = new Block((BlockType)randList[2], upPic3, blockImage);
            gameArr[1][3] = new Block((BlockType)randList[3], upPic4, blockImage);
            gameArr[1][4] = new Block((BlockType)randList[4], upPic5, blockImage);
            gameArr[1][5] = new Block((BlockType)randList[5], upPic6, blockImage);
            gameArr[2][3] = new Block((BlockType)randList[6], upPic7, blockImage);
            gameArr[2][4] = new Block((BlockType)randList[7], upPic8, blockImage);
            gameArr[2][5] = new Block((BlockType)randList[8], upPic9, blockImage);

            gameArr[3][0] = new Block((BlockType)randList[9], leftPic1, blockImage);
            gameArr[3][1] = new Block((BlockType)randList[10], leftPic2, blockImage);
            gameArr[3][2] = new Block((BlockType)randList[11], leftPic3, blockImage);
            gameArr[4][0] = new Block((BlockType)randList[12], leftPic4, blockImage);
            gameArr[4][1] = new Block((BlockType)randList[13], leftPic5, blockImage);
            gameArr[4][2] = new Block((BlockType)randList[14], leftPic6, blockImage);
            gameArr[5][0] = new Block((BlockType)randList[15], leftPic7, blockImage);
            gameArr[5][1] = new Block((BlockType)randList[16], leftPic8, blockImage);
            gameArr[5][2] = new Block((BlockType)randList[17], leftPic9, blockImage);

            gameArr[3][3] = new Block((BlockType)randList[18], centralPic1, blockImage);
            gameArr[3][4] = new Block((BlockType)randList[19], centralPic2, blockImage);
            gameArr[3][5] = new Block((BlockType)randList[20], centralPic3, blockImage);
            gameArr[4][3] = new Block((BlockType)randList[21], centralPic4, blockImage);
            gameArr[4][4] = new Block((BlockType)randList[22], centralPic5, blockImage);
            gameArr[4][5] = new Block((BlockType)randList[23], centralPic6, blockImage);
            gameArr[5][3] = new Block((BlockType)randList[24], centralPic7, blockImage);
            gameArr[5][4] = new Block((BlockType)randList[25], centralPic8, blockImage);
            gameArr[5][5] = new Block((BlockType)randList[26], centralPic9, blockImage);

            gameArr[3][6] = new Block((BlockType)randList[27], rightPic1, blockImage);
            gameArr[3][7] = new Block((BlockType)randList[28], rightPic2, blockImage);
            gameArr[3][8] = new Block((BlockType)randList[29], rightPic3, blockImage);
            gameArr[4][6] = new Block((BlockType)randList[30], rightPic4, blockImage);
            gameArr[4][7] = new Block((BlockType)randList[31], rightPic5, blockImage);
            gameArr[4][8] = new Block((BlockType)randList[32], rightPic6, blockImage);
            gameArr[5][6] = new Block((BlockType)randList[33], rightPic7, blockImage);
            gameArr[5][7] = new Block((BlockType)randList[34], rightPic8, blockImage);
            gameArr[5][8] = new Block((BlockType)randList[35], rightPic9, blockImage);

            gameArr[6][3] = new Block((BlockType)randList[36], downPic1, blockImage);
            gameArr[6][4] = new Block((BlockType)randList[37], downPic2, blockImage);
            gameArr[6][5] = new Block((BlockType)randList[38], downPic3, blockImage);
            gameArr[7][3] = new Block((BlockType)randList[39], downPic4, blockImage);
            gameArr[7][4] = new Block((BlockType)randList[40], downPic5, blockImage);
            gameArr[7][5] = new Block((BlockType)randList[41], downPic6, blockImage);
            gameArr[8][3] = new Block((BlockType)randList[42], downPic7, blockImage);
            gameArr[8][4] = new Block((BlockType)randList[43], downPic8, blockImage);
            gameArr[8][5] = new Block((BlockType)randList[44], downPic9, blockImage);
            StepCount = 0;
            SecCount = 0;
            pause_button.Text = "Pause";
            setArrowBtnStatus(true);
            if (timerThread != null)
            {
                if (timerThread.IsAlive)
                {
                    timerThread.Abort();
                }
            }
            timerThread = new Thread(GameTimer);
            timerThread.Start();
    }

        private int StepCount
        {
            get => stepCount;
            set
            {
                stepCount = value;
                count_text.Text = stepCount.ToString();
            }
        }

        private int SecCount
        {
            get => secCount;
            set
            {
                secCount = value;
                try
                {
                    sec_text.Invoke((MethodInvoker)delegate { sec_text.Text = secCount.ToString(); });
                }
                catch { }
            }
        }

        private void GameTimer()
        {
            while (true)
            {
                Thread.Sleep(1000);
                SecCount++;
            }
        }
        private bool isWin()
        {
            BlockType temp = gameArr[3][3].BlockType;
            for (int y = 3; y < 6; y++) 
            {
                for (int x = 3; x < 6; x++)
                {
                    if (gameArr[x][y].BlockType != temp) return false;
                }
            }
            return true;
        }

        private void checkWin()
        {
            if (isWin())
            {
                if (timerThread != null)
                {
                    if (timerThread.IsAlive)
                    {
                        timerThread.Abort();
                    }
                }
                MessageBox.Show("Congratulation!!!\nClear Time: " + sec_text.Text + "s\nTotal Steps: " + count_text.Text, "Congratulation", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                GameStart();
            }
        }

        private void UpBtnKeyClick(int x)
        {
            if (x < 3 || x > 6) return;
            BlockType temp = gameArr[0][x].BlockType;
            for (int y = 0; y < 8; y++)
            {
                gameArr[y][x].BlockType = gameArr[y + 1][x].BlockType;
            }
            gameArr[8][x].BlockType = temp;
            StepCount++;
            checkWin();
        }

        private void DownBtnKeyClick(int x)
        {
            if (x < 3 || x > 6) return;
            BlockType temp = gameArr[8][x].BlockType;
            for (int y = 8; y >= 1; y--)
            {
                gameArr[y][x].BlockType = gameArr[y - 1][x].BlockType;
            }
            gameArr[0][x].BlockType = temp;
            StepCount++;
            checkWin();
        }

        private void LeftBtnKeyClick(int y)
        {
            if (y < 3 || y > 6) return;
            BlockType temp = gameArr[y][0].BlockType;
            for (int x = 0; x < 8; x++)
            {
                gameArr[y][x].BlockType = gameArr[y][x + 1].BlockType;
            }
            gameArr[y][8].BlockType = temp;
            StepCount++;
            checkWin();
        }

        private void RightBtnKeyClick(int y)
        {
            if (y < 3 || y > 6) return;
            BlockType temp = gameArr[y][8].BlockType;
            for (int x = 8; x >= 1; x--)
            {
                gameArr[y][x].BlockType = gameArr[y][x - 1].BlockType;
            }
            gameArr[y][0].BlockType = temp;
            StepCount++;
            checkWin();
        }

        private void up_btn1_4_Click(object sender, EventArgs e)
        {
            UpBtnKeyClick(3);
        }

        private void up_btn2_5_Click(object sender, EventArgs e)
        {
            UpBtnKeyClick(4);
        }

        private void up_btn3_6_Click(object sender, EventArgs e)
        {
            UpBtnKeyClick(5);
        }

        private void left_btn1_4_Click(object sender, EventArgs e)
        {
            LeftBtnKeyClick(3);
        }

        private void left_btn2_5_Click(object sender, EventArgs e)
        {
            LeftBtnKeyClick(4);
        }

        private void left_btn3_6_Click(object sender, EventArgs e)
        {
            LeftBtnKeyClick(5);
        }

        private void down_btn1_4_Click(object sender, EventArgs e)
        {
            DownBtnKeyClick(3);
        }

        private void down_btn2_5_Click(object sender, EventArgs e)
        {
            DownBtnKeyClick(4);
        }

        private void down_btn3_6_Click(object sender, EventArgs e)
        {
            DownBtnKeyClick(5);
        }

        private void right_btn1_4_Click(object sender, EventArgs e)
        {
            RightBtnKeyClick(3);
        }

        private void right_btn2_5_Click(object sender, EventArgs e)
        {
            RightBtnKeyClick(4);
        }

        private void right_btn3_6_Click(object sender, EventArgs e)
        {
            RightBtnKeyClick(5);
        }

        private void restart_btn_Click(object sender, EventArgs e)
        {
            GameStart();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(Environment.ExitCode);
        }

        private void setArrowBtnStatus(bool status)
        {
            up_btn1.Enabled = status;
            up_btn2.Enabled = status;
            up_btn3.Enabled = status;
            up_btn4.Enabled = status;
            up_btn5.Enabled = status;
            up_btn6.Enabled = status;
            down_btn1.Enabled = status;
            down_btn2.Enabled = status;
            down_btn3.Enabled = status;
            down_btn4.Enabled = status;
            down_btn5.Enabled = status;
            down_btn6.Enabled = status;
            left_btn1.Enabled = status;
            left_btn2.Enabled = status;
            left_btn3.Enabled = status;
            left_btn4.Enabled = status;
            left_btn5.Enabled = status;
            left_btn6.Enabled = status;
            right_btn1.Enabled = status;
            right_btn2.Enabled = status;
            right_btn3.Enabled = status;
            right_btn4.Enabled = status;
            right_btn5.Enabled = status;
            right_btn6.Enabled = status;
        }

        private void pause_button_Click(object sender, EventArgs e)
        {
            if (pause_button.Text.Equals("Pause"))
            {
                setArrowBtnStatus(false);
                if (timerThread != null)
                {
                    if (timerThread.IsAlive)
                    {
                        timerThread.Abort();
                    }
                }
                pause_button.Text = "Continue";
            }
            else
            {
                setArrowBtnStatus(true);
                timerThread = new Thread(GameTimer);
                timerThread.Start();
                pause_button.Text = "Pause";
            }
        }
    }
}
