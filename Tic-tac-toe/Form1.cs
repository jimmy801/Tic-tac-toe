using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1043319_HW_2
{
    public partial class Tic_Tac_Toe : Form
    {
        Rectangle[] rect = new Rectangle[9]; // 9個矩陣陣列
        Random num = new Random(); // 隨機亂數
        bool isCircle = true; // 判斷玩家要"O"還是"X"
        bool isFirst = true; // 判斷玩家先下還是後下
        bool go = true; // 判斷還可不可以繼續下
        bool the5 = false; // 判斷玩家有沒有下角落
        bool CenterOrCorner = true; // 電腦先手，決定要採用中心策略或角落策略
        bool CornerStrategy = false; // 角落策略的策略1或策略2
        bool Choice = false; // 角落策略2的隨機佔角
        int[] table = new int[9]; // 每個矩形的狀態
        int[] empty = new int[9]; // 還可以被下的矩形
        int[] StepsList = new int[9]; // 步數的順序
        int steps = 0; // 總步數
        int[] corner = { 0, 2, 6, 8 };// 指向四角
        int[] side = { 1, 3, 5, 7 };// 指向四邊


        public Tic_Tac_Toe()
        {
            InitializeComponent(); // 初始化視窗

            for (int i = 0; i < 9; ++i)
            {
                // 所有的矩形都設為長寬60
                rect[i].Height = 60;
                rect[i].Width = 60;
                rect[i].X = i % 3 * 60;
                rect[i].Y = i / 3 * 60 + 30;

                // 把所有矩形狀態歸零
                table[i] = 0;
            }
        }

        // 玩家先下
        private void button6_Click(object sender, EventArgs e)
        {
            First_or_Last();
        }

        // 玩家後下
        private void button7_Click(object sender, EventArgs e)
        {
            isFirst = false;
            if (num.Next(0, 2) == 1) CenterOrCorner = false;
            First_or_Last();
        }

        // 玩家隨機順序
        private void button8_Click(object sender, EventArgs e)
        {
            if (num.Next(0, 2) == 1) isFirst = false;
            First_or_Last();
        }

        // 玩家決定順序後的處理
        private void First_or_Last()
        {
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            label3.Text = "";
            label4.Text = "Player want";
        }

        // 玩家要用"O"
        private void button3_Click(object sender, EventArgs e)
        {
            AllButtonClick();
        }

        // 玩家要用"X"
        private void button4_Click(object sender, EventArgs e)
        {
            isCircle = false;
            AllButtonClick();
        }

        // 玩家的隨機
        private void button5_Click(object sender, EventArgs e)
        {
            if (num.Next(0, 2) == 1) isCircle = false;
            AllButtonClick();
        }

        // 讓所有按鍵消失並畫矩形
        private void AllButtonClick()
        {
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            label4.Text = "";
            this.CreateGraphics().DrawRectangles(new Pen(Color.Black, 5), rect); // 畫矩形

            if (!isFirst) Sente(9); // 如果電腦先要先讓電腦下
        }

        // 判斷是否結束遊戲
        private bool over(Graphics g)
        {
            for (int i = 0; i < 3; ++i)
            {
                // 一行一行判斷
                if (table[i * 3 + 0] > 0 && table[i * 3 + 0] == table[i * 3 + 1] && table[i * 3 + 2] == table[i * 3 + 0])
                {
                    if (table[i * 3 + 0] > 1) label1.Text = "You Lose!";
                    else label1.Text = "You Win!";
                    for (int j = 0; j < 9; ++j) Draw_Picture(CreateGraphics(), j);
                    g.DrawLine(new Pen(Color.Green, 4), new Point(rect[i * 3].X + 10, rect[i * 3].Y + 30), new Point(rect[i * 3 + 2].X + 48, rect[i * 3 + 2].Y + 30));
                    button2.Visible = false; // 遊戲結束不能再Undo
                    return true;
                }

                // 一列一列判斷
                if (table[0 + i] > 0 && table[0 + i] == table[3 + i] && table[6 + i] == table[0 + i])
                {
                    if (table[0 + i] > 1) label1.Text = "You Lose!";
                    else label1.Text = "You Win!";
                    for (int j = 0; j < 9; ++j) Draw_Picture(CreateGraphics(), j);
                    g.DrawLine(new Pen(Color.Green, 4), new Point(rect[0 + i].X + 30, rect[0 + i].Y + 10), new Point(rect[6 + i].X + 30, rect[6 + i].Y + 50));
                    button2.Visible = false; // 遊戲結束不能再Undo
                    return true;
                }
            }

            // 判斷斜線
            if (table[4] > 0 && (table[0] == table[4] && table[8] == table[4]))
            {
                if (table[4] > 1) label1.Text = "You Lose!";
                else label1.Text = "You Win!";
                for (int j = 0; j < 9; ++j) Draw_Picture(CreateGraphics(), j);
                g.DrawLine(new Pen(Color.Green, 4), new Point(rect[0].X + 10, rect[0].Y + 10), new Point(rect[8].X + 50, rect[8].Y + 50));
                button2.Visible = false; // 遊戲結束不能再Undo
                return true;
            }
            if (table[4] > 0 && (table[2] == table[4] && table[6] == table[4]))
            {
                if (table[4] > 1) label1.Text = "You Lose!";
                else label1.Text = "You Win!";
                for (int j = 0; j < 9; ++j) Draw_Picture(CreateGraphics(), j);
                g.DrawLine(new Pen(Color.Green, 4), new Point(rect[2].X + 50, rect[2].Y + 10), new Point(rect[6].X + 10, rect[6].Y + 50));
                button2.Visible = false; // 遊戲結束不能再Undo
                return true;
            }

            // 平手
            if (steps == 9)
            {
                label1.Text = "Tie";
                for (int j = 0; j < 9; ++j) Draw_Picture(CreateGraphics(), j);
                button2.Visible = false; // 遊戲結束不能再Undo
                return true;
            }

            return false;
        }

        // 畫出下子狀況
        private void Draw_Picture(Graphics g, int n)
        {
            Rectangle r = rect[n];

            if (table[n] == 1) // 玩家
            {
                if (isCircle) g.DrawEllipse(new Pen(Color.Blue, 3), r.X + 8, r.Y + 8, 45, 45);
                else
                {
                    g.DrawLine(new Pen(Color.Red, 3), r.X + 15, r.Y + 15, r.X + 45, r.Y + 45);
                    g.DrawLine(new Pen(Color.Red, 3), r.X + 45, r.Y + 15, r.X + 15, r.Y + 45);
                }
            }
            else if (table[n] > 1) // 電腦
            {
                if (isCircle)
                {
                    g.DrawLine(new Pen(Color.Red, 3), r.X + 15, r.Y + 15, r.X + 45, r.Y + 45);
                    g.DrawLine(new Pen(Color.Red, 3), r.X + 45, r.Y + 15, r.X + 15, r.Y + 45);
                }
                else g.DrawEllipse(new Pen(Color.Blue, 3), r.X + 8, r.Y + 8, 45, 45);
            }
            g.DrawRectangles(new Pen(Color.Black, 5), rect);
        }

        // 電腦自動判斷下子位置
        private void Computer_Move(int EmptyTotal)
        {
            // 斜線
            int[] ForwardSlash = { 2, 4, 6 };
            // 反斜線
            int[] BackSlash = { 0, 4, 8 };

            // 優先取得勝利
            for (int i = 0; i < 3; ++i)
            {
                // 一行一行判斷
                if ((table[i * 3 + 1] == 2 && table[i * 3 + 0] == table[i * 3 + 1]) || (table[i * 3 + 2] == 2 && table[i * 3 + 2] == table[i * 3 + 0]) || (table[i * 3 + 1] == 2 && table[i * 3 + 2] == table[i * 3 + 1]))
                    for (int j = 0; j < 3; ++j) if (table[i * 3 + j] == 0) { table[i * 3 + j] = 2; StepsList[steps++] = i * 3 + j; Draw_Picture(CreateGraphics(), i * 3 + j); return; }

                // 一列一列判斷
                if ((table[3 + i] == 2 && table[0 + i] == table[3 + i]) || (table[6 + i] == 2 && table[6 + i] == table[0 + i]) || (table[3 + i] == 2 && table[3 + i] == table[6 + i]))
                    for (int j = 0; j < 3; ++j) if (table[j * 3 + i] == 0) { table[j * 3 + i] = 2; StepsList[steps++] = j * 3 + i; Draw_Picture(CreateGraphics(), j * 3 + i); return; }

                // 判斷斜線
                for (int j = i + 1; j < 3; ++j)
                {
                    if (table[ForwardSlash[i]] == 2 && table[ForwardSlash[j]] == table[ForwardSlash[i]])
                        for (int k = 0; k < 3; ++k) if (table[ForwardSlash[k]] == 0) { table[ForwardSlash[k]] = 2; StepsList[steps++] = ForwardSlash[k]; Draw_Picture(CreateGraphics(), ForwardSlash[k]); return; }
                    if (table[BackSlash[i]] == 2 && table[BackSlash[j]] == table[BackSlash[i]])
                        for (int k = 0; k < 3; ++k) if (table[BackSlash[k]] == 0) { table[BackSlash[k]] = 2; StepsList[steps++] = BackSlash[k]; Draw_Picture(CreateGraphics(), BackSlash[k]); return; }
                }
            }

            /*---------------------------------------------------------------------*/

            // 阻擋
            for (int i = 0; i < 3; ++i)
            {
                // 一行一行判斷
                if ((table[i * 3 + 0] == table[i * 3 + 1] && table[i * 3 + 2] == 0 && table[i * 3 + 0] > 0) || (table[i * 3 + 2] == table[i * 3 + 0] && table[i * 3 + 1] == 0 && table[i * 3 + 2] > 0) || (table[i * 3 + 1] == table[i * 3 + 2] && table[i * 3 + 0] == 0 && table[i * 3 + 1] > 0))
                    for (int j = 0; j < 3; ++j) if (table[i * 3 + j] == 0) { table[i * 3 + j] = 2; StepsList[steps++] = i * 3 + j; Draw_Picture(CreateGraphics(), i * 3 + j); return; }

                // 一列一列判斷
                if ((table[0 + i] == table[3 + i] && table[6 + i] == 0 && table[0 + i] > 0) || (table[6 + i] == table[0 + i] && table[3 + i] == 0 && table[6 + i] > 0) || (table[3 + i] == table[6 + i] && table[0 + i] == 0 && table[3 + i] > 0))
                    for (int j = 0; j < 3; ++j) if (table[j * 3 + i] == 0) { table[j * 3 + i] = 2; StepsList[steps++] = j * 3 + i; Draw_Picture(CreateGraphics(), j * 3 + i); return; }

                // 判斷斜線
                for (int j = i + 1; j < 3; ++j)
                {
                    if (table[ForwardSlash[i]] == 1 && table[ForwardSlash[j]] == table[ForwardSlash[i]])
                        for (int k = 0; k < 3; ++k) if (table[ForwardSlash[k]] == 0) { table[ForwardSlash[k]] = 2; StepsList[steps++] = ForwardSlash[k]; Draw_Picture(CreateGraphics(), ForwardSlash[k]); return; }
                    if (table[BackSlash[i]] == 1 && table[BackSlash[j]] == table[BackSlash[i]])
                        for (int k = 0; k < 3; ++k) if (table[BackSlash[k]] == 0) { table[BackSlash[k]] = 2; StepsList[steps++] = BackSlash[k]; Draw_Picture(CreateGraphics(), BackSlash[k]); return; }
                }
            }

            /*---------------------------------------------------------------------*/

            // 無法取得勝利或沒有需要阻擋則隨意動,或特殊策略
            if (CornerStrategy && steps == 4)
            {
                for (int i = 0; i < 4; ++i)
                    if (table[corner[i]] == 2)
                    {
                        switch (i)
                        {
                            case 0:
                                if (Choice)
                                {
                                    table[6] = 2;
                                    StepsList[steps++] = 6;
                                    Draw_Picture(CreateGraphics(), 6);
                                }
                                else
                                {
                                    table[2] = 2;
                                    StepsList[steps++] = 2;
                                    Draw_Picture(CreateGraphics(), 2);
                                }
                                break;
                            case 1:
                                if (Choice)
                                {
                                    table[0] = 2;
                                    StepsList[steps++] = 0;
                                    Draw_Picture(CreateGraphics(), 0);
                                }
                                else
                                {
                                    table[8] = 2;
                                    StepsList[steps++] = 8;
                                    Draw_Picture(CreateGraphics(), 8);
                                }
                                break;
                            case 2:
                                if (Choice)
                                {
                                    table[8] = 2;
                                    StepsList[steps++] = 8;
                                    Draw_Picture(CreateGraphics(), 8);
                                }
                                else
                                {
                                    table[0] = 2;
                                    StepsList[steps++] = 0;
                                    Draw_Picture(CreateGraphics(), 0);
                                }
                                break;
                            case 3:
                                if (Choice)
                                {
                                    table[2] = 2;
                                    StepsList[steps++] = 2;
                                    Draw_Picture(CreateGraphics(), 2);
                                }
                                else
                                {
                                    table[6] = 2;
                                    StepsList[steps++] = 6;
                                    Draw_Picture(CreateGraphics(), 6);
                                }
                                break;
                        }
                        break;
                    }
                return;
            }
            else
            {
                int Rtmp = num.Next(0, EmptyTotal);
                table[empty[Rtmp]] = 2;
                StepsList[steps++] = empty[Rtmp];
                Draw_Picture(CreateGraphics(), empty[Rtmp]);
            }
        }

        // 當滑鼠按下時觸發事件
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int j = 0;
            if (e.Location.X < 178 && e.Location.Y < 210 && e.Location.Y > 30) // 要按在9個矩形內才觸發
            {
                for (int i = 0; i < 9; ++i)
                    if (go) // 還能下子才執行
                    {
                        if (table[i] == 0)
                        {
                            if (rect[i].Contains(e.Location)) // 如果按到的位置是可以下子的地方就下子
                            {
                                table[i] = 1;
                                StepsList[steps++] = i;
                                Draw_Picture(CreateGraphics(), i);
                            }
                            else empty[j++] = i; // 如果該矩形可以下子，但不是玩家按的地方就紀錄
                        }
                        else if (rect[i].Contains(e.Location)) return; // 如果按下的矩形已被下子了就重下
                    }
                if (!over(CreateGraphics())) { if (isFirst) Gote(j); else Sente(j); } // 遊戲還沒結束就讓電腦下
                if (over(CreateGraphics())) { label2.Text = "Again?"; button1.Visible = true; go = false; } // 遊戲結束
            }
        }

        // 電腦先手的第3步(總步數第5步)需要採用特殊手段
        private void fifth(int EmptyTotal)
        {
            // 判斷四個角是否都被占據並佔據角落
            for (int i = 0; i < 4; ++i)
                if (table[corner[i]] == 1)
                {
                    switch (i)
                    {
                        case 0:
                            if (table[5] > 0) { table[6] = 2; StepsList[steps++] = 6; Draw_Picture(CreateGraphics(), 6); }
                            else if (table[7] > 0) { table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                            else Computer_Move(EmptyTotal);
                            break;
                        case 1:
                            if (table[3] > 0) { table[8] = 2; StepsList[steps++] = 8; Draw_Picture(CreateGraphics(), 8); }
                            else if (table[7] > 0) { table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                            else Computer_Move(EmptyTotal);
                            break;
                        case 2:
                            if (table[1] > 0) { table[8] = 2; StepsList[steps++] = 8; Draw_Picture(CreateGraphics(), 8); }
                            else if (table[5] > 0) { table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                            else Computer_Move(EmptyTotal);
                            break;
                        case 3:
                            if (table[1] > 0) { table[6] = 2; StepsList[steps++] = 6; Draw_Picture(CreateGraphics(), 6); }
                            else if (table[3] > 0) { table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                            else Computer_Move(EmptyTotal);
                            break;
                    }
                    break;
                }
        }

        // 電腦先手
        private void Sente(int EmptyTotal)
        {
            // 佔據中心策略
            if (CenterOrCorner)
            {
                // 電腦第1步先佔據中心
                if (steps == 0) { table[4] = 2; StepsList[steps++] = 4; Draw_Picture(CreateGraphics(), 4); }
                // 電腦第2步(總步數第3步)
                else if (steps == 2)
                {
                    bool s = false; // 判斷玩家是否佔據邊界
                    int i = 0; // 紀錄玩家佔據哪個邊
                    for (; i < 4; ++i) if (table[side[i]] > 0) { s = true; break; }

                    // 如果玩家有佔據邊界
                    if (s)
                    {
                        int temR = num.Next(0, 2);
                        // 除了玩家佔據的邊界左右兩個角之外的角落隨機佔據一個
                        switch (i)
                        {
                            case 0:
                                if (temR == 1) { table[6] = 2; StepsList[steps++] = 6; Draw_Picture(CreateGraphics(), 6); }
                                else { table[8] = 2; StepsList[steps++] = 8; Draw_Picture(CreateGraphics(), 8); }
                                break;
                            case 1:
                                if (temR == 1) { table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                                else { table[8] = 2; StepsList[steps++] = 8; Draw_Picture(CreateGraphics(), 8); }
                                break;
                            case 2:
                                if (temR == 1) { table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                                else { table[6] = 2; StepsList[steps++] = 6; Draw_Picture(CreateGraphics(), 6); }
                                break;
                            case 3:
                                if (temR == 1) { table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                                else { table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                                break;
                        }
                    }
                    // 如果玩家沒佔據邊界
                    else
                    {

                        for (i = 0; i < 4; ++i)
                            if (table[corner[i]] > 0)
                            {
                                // 依照玩家佔據的角落決定接下來要佔據哪個角落
                                switch (i)
                                {
                                    case 0:
                                        table[8] = 2;
                                        StepsList[steps++] = 8;
                                        Draw_Picture(CreateGraphics(), 8);
                                        break;
                                    case 1:
                                        table[6] = 2;
                                        StepsList[steps++] = 6;
                                        Draw_Picture(CreateGraphics(), 6);
                                        break;
                                    case 2:
                                        table[2] = 2;
                                        StepsList[steps++] = 2;
                                        Draw_Picture(CreateGraphics(), 2);
                                        break;
                                    case 3:
                                        table[0] = 2;
                                        StepsList[steps++] = 0;
                                        Draw_Picture(CreateGraphics(), 0);
                                        break;
                                }
                                the5 = true; // 接下來電腦的第3步(總步數第5步)要採用特殊手段
                                break;
                            }
                    }
                }
                // 電腦的第3步(總步數第5步)要採用特殊手段
                else if (the5 && steps == 4) fifth(EmptyTotal);

                // 其他步數
                else Computer_Move(EmptyTotal);
            }

            // 佔據角落策略
            else
            {
                if (steps == 0)
                {
                    int tmpR = num.Next(0, 4);
                    table[corner[tmpR]] = 2;
                    StepsList[steps++] = corner[tmpR];
                    Draw_Picture(CreateGraphics(), corner[tmpR]);
                }
                else
                {
                    if (steps == 2)
                    {
                        if (table[4] > 0) // 如果玩家佔據中心
                        {
                            int i;
                            for (i = 0; i < 4; ++i) if (table[corner[i]] > 0) break;
                            int R = num.Next(0, 2);
                            if (R == 1) CornerStrategy = true; // 角落策略二

                            if (!CornerStrategy) // 角落策略一
                                switch (i)
                                {
                                    case 0:
                                        table[8] = 2;
                                        StepsList[steps++] = 8;
                                        Draw_Picture(CreateGraphics(), 8);
                                        break;
                                    case 1:
                                        table[6] = 2;
                                        StepsList[steps++] = 6;
                                        Draw_Picture(CreateGraphics(), 6);
                                        break;
                                    case 2:
                                        table[2] = 2;
                                        StepsList[steps++] = 2;
                                        Draw_Picture(CreateGraphics(), 2);
                                        break;
                                    case 3:
                                        table[0] = 2;
                                        StepsList[steps++] = 0;
                                        Draw_Picture(CreateGraphics(), 0);
                                        break;
                                }

                            else // 角落策略二
                            {
                                R = num.Next(0, 2);
                                if (R == 1) Choice = true;

                                switch (i)
                                {
                                    case 0:
                                        if (Choice)
                                        {
                                            table[7] = 2;
                                            StepsList[steps++] = 7;
                                            Draw_Picture(CreateGraphics(), 7);
                                        }
                                        else
                                        {
                                            table[5] = 2;
                                            StepsList[steps++] = 5;
                                            Draw_Picture(CreateGraphics(), 5);
                                        }
                                        break;
                                    case 1:
                                        if (Choice)
                                        {
                                            table[3] = 2;
                                            StepsList[steps++] = 3;
                                            Draw_Picture(CreateGraphics(), 3);
                                        }
                                        else
                                        {
                                            table[7] = 2;
                                            StepsList[steps++] = 7;
                                            Draw_Picture(CreateGraphics(), 7);
                                        }
                                        break;
                                    case 2:
                                        if (Choice)
                                        {
                                            table[5] = 2;
                                            StepsList[steps++] = 5;
                                            Draw_Picture(CreateGraphics(), 5);
                                        }
                                        else
                                        {
                                            table[1] = 2;
                                            StepsList[steps++] = 1;
                                            Draw_Picture(CreateGraphics(), 1);
                                        }
                                        break;
                                    case 3:
                                        if (Choice)
                                        {
                                            table[1] = 2;
                                            StepsList[steps++] = 1;
                                            Draw_Picture(CreateGraphics(), 1);
                                        }
                                        else
                                        {
                                            table[3] = 2;
                                            StepsList[steps++] = 3;
                                            Draw_Picture(CreateGraphics(), 3);
                                        }
                                        break;
                                }
                            }
                        }
                        else // 如果玩家沒佔據中心
                        {
                            int i = 0;
                            for (; i < 4; i++) if (table[corner[i]] == 2) break;
                            switch (i)
                            {
                                case 0:
                                    if (table[1] > 0 || table[2] > 0) { table[6] = 2; StepsList[steps++] = 6; Draw_Picture(CreateGraphics(), 6); }
                                    else { table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                                    break;
                                case 1:
                                    if (table[1] > 0 || table[0] > 0) { table[8] = 2; StepsList[steps++] = 8; Draw_Picture(CreateGraphics(), 8); }
                                    else { table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                                    break;
                                case 2:
                                    if (table[7] > 0 || table[8] > 0) { table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                                    else { table[8] = 2; StepsList[steps++] = 8; Draw_Picture(CreateGraphics(), 8); }
                                    break;
                                case 3:
                                    if (table[6] > 0 || table[7] > 0) { table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                                    else { table[6] = 2; StepsList[steps++] = 6; Draw_Picture(CreateGraphics(), 6); }
                                    break;
                            }
                        }
                    }
                    else if (steps == 4)
                    {
                        if (table[4] == 0)
                        {
                            if ((((table[0] == table[2] && table[1] == 0) ||
                                (table[0] == table[6] && table[3] == 0))
                                && table[0] == 2) ||
                                (((table[8] == table[2] && table[5] == 0) ||
                                (table[8] == table[6] && table[7] == 0))
                                && table[8] == 2)) Computer_Move(EmptyTotal);
                            else if ((table[0] == table[8] && table[0] > 0) || (table[2] == table[6] && table[2] > 0)) Computer_Move(EmptyTotal);
                            else
                            {
                                bool[] capable = { false, false, false, false, false, false, false, false, false };
                                for (int i = 0; i < 4; ++i)
                                {
                                    if (table[side[i]] > 0)
                                        switch (i)
                                        {
                                            case 0:
                                                capable[0] = capable[2] = true;
                                                break;
                                            case 1:
                                                capable[0] = capable[6] = true;
                                                break;
                                            case 2:
                                                capable[8] = capable[2] = true;
                                                break;
                                            case 3:
                                                capable[6] = capable[8] = true;
                                                break;
                                        }
                                }
                                bool b = false;
                                for (int i = 0; i < 3; i += 2)
                                {
                                    for (int j = 0; j < 3; j += 2)
                                        if (!capable[i * 3 + j] && table[i * 3 + j] == 0)
                                        {
                                            table[i * 3 + j] = 2;
                                            StepsList[steps++] = i * 3 + j;
                                            Draw_Picture(CreateGraphics(), i * 3 + j);
                                            b = true;
                                            break;
                                        }
                                    if (b) break;
                                }
                            }
                        }
                        else
                        {
                            int allCroner = 0, last = 0;
                            for (int i = 0; i < 4; ++i)
                            {
                                if (table[corner[i]] > 0) allCroner++;
                                else last = i;
                            }
                            if (allCroner > 2) { table[corner[last]] = 2; StepsList[steps++] = corner[last]; Draw_Picture(CreateGraphics(), corner[last]); }
                            else Computer_Move(EmptyTotal);
                        }
                    }
                    else Computer_Move(EmptyTotal);
                }
            }
        }

        // 電腦後手
        private void Gote(int EmptyTotal)
        {
            // 如果電腦後手的第1步(總步數第2步)
            if (steps == 1)
            {
                // 如果中心被玩家佔據就隨機佔據四角之一
                if (table[4] > 0) { int r = num.Next(0, 4); table[corner[r]] = 2; StepsList[steps++] = corner[r]; Draw_Picture(CreateGraphics(), corner[r]); }
                // 如果玩家沒佔據中心則優先佔據中心
                else { table[4] = 2; StepsList[steps++] = 4; Draw_Picture(CreateGraphics(), 4); }
            }
            // 如果電腦後手的第2步(總步數第4步)
            else if (steps == 3)
            {
                // 如果中心是玩家佔據
                if (table[4] == 1)
                {
                    bool humanConer = false; // 判斷玩家有沒有佔據角落
                    for (int i = 0; i < 4; ++i)
                        if (table[corner[i]] == 1)
                        {
                            switch (i)
                            {
                                case 0:
                                    // 另一角是電腦佔據則用特殊策略，否則電腦自動下子
                                    if (table[8] > 0) humanConer = true;
                                    break;
                                case 1:
                                    // 另一角是電腦佔據則用特殊策略，否則電腦自動下子
                                    if (table[6] > 0) humanConer = true;
                                    break;
                                case 2:
                                    // 另一角是電腦佔據則用特殊策略，否則電腦自動下子
                                    if (table[2] > 0) humanConer = true;
                                    break;
                                case 3:
                                    // 另一角是電腦佔據則用特殊策略，否則電腦自動下子
                                    if (table[0] > 0) humanConer = true;
                                    break;
                            }
                            break;
                        }

                    // 如果玩家也有佔據角落，而且沒機會連成斜線(另一角是電腦佔據)
                    if (humanConer)
                    {
                        int Rtmp = num.Next(0, 4);
                        // 電腦隨機佔據一個角落
                        if (table[corner[Rtmp]] == 0) { table[corner[Rtmp]] = 2; StepsList[steps++] = corner[Rtmp]; Draw_Picture(CreateGraphics(), corner[Rtmp]); }
                        // 如果隨機的角落已經被佔據，則隨機佔據剩餘的2個角落之一
                        else
                        {
                            int[] tmp = new int[2];
                            for (int i = 0, k = 0; i < 4; ++i)
                                if (table[corner[i]] == 0) tmp[k++] = i;
                            int r = num.Next(0, 2);
                            table[corner[tmp[r]]] = 2;
                            StepsList[steps++] = corner[tmp[r]];
                            Draw_Picture(CreateGraphics(), corner[tmp[r]]);
                        }
                    }

                        // 其餘情形讓電腦自己下子
                    else Computer_Move(EmptyTotal);

                }
                // 如果中心是電腦佔據
                else
                {
                    if (((table[0] == table[2] || table[0] == table[6]) && table[0] == 1) ||
                        ((table[8] == table[2] || table[8] == table[6]) && table[8] == 1))
                        Computer_Move(EmptyTotal);

                    else if (((table[0] == table[1] || table[1] == table[2]) && table[1] == 1) ||
                            ((table[3] == table[4] || table[4] == table[5]) && table[4] == 1) ||
                        ((table[6] == table[7] || table[7] == table[8]) && table[7] == 1) ||
                        ((table[0] == table[3] || table[3] == table[6]) && table[3] == 1) ||
                        ((table[2] == table[5] || table[5] == table[8]) && table[5] == 1))
                        Computer_Move(EmptyTotal);

                    else if (table[1] == 1 || table[7] == 1)
                    {
                        if (table[3] == 1) { table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                        else if (table[5] == 1) { table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                        // 其餘情形讓電腦自己下子
                        else Computer_Move(EmptyTotal);
                    }

                    else
                    {
                        bool humanConer = false; // 判斷玩家有沒有佔據角落
                        for (int i = 0; i < 4; ++i)
                            if (table[corner[i]] == 1)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (table[5] > 0) { humanConer = true; table[2] = 2; StepsList[steps++] = 2; Draw_Picture(CreateGraphics(), 2); }
                                        break;
                                    case 1:
                                        if (table[3] > 0) { humanConer = true; table[0] = 2; StepsList[steps++] = 0; Draw_Picture(CreateGraphics(), 0); }
                                        break;
                                    case 2:
                                        if (table[5] > 0) { humanConer = true; table[8] = 2; StepsList[steps++] = 8; Draw_Picture(CreateGraphics(), 8); }
                                        break;
                                    case 3:
                                        if (table[3] > 0) { humanConer = true; table[6] = 2; StepsList[steps++] = 6; Draw_Picture(CreateGraphics(), 6); }
                                        break;
                                }
                                break;
                            }


                        if (!humanConer)
                        {
                            int r = num.Next(0, 4);
                            while (table[side[r]] > 0) r = num.Next(0, 4);
                            table[side[r]] = 2;
                            StepsList[steps++] = side[r];
                            Draw_Picture(CreateGraphics(), side[r]);
                        }
                    }
                }
            }
            // 剩下其他步數都讓電腦自己決定
            else Computer_Move(EmptyTotal);
        }

        // 按下狀態列的Reset
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        // 按下視窗內的Reset
        private void button1_Click(object sender, EventArgs e)
        {
            Reset();
        }

        // Reset
        private void Reset()
        {
            Graphics g1 = this.CreateGraphics();
            g1.Clear(this.BackColor);
            label1.Text = "";
            label2.Text = "";
            label3.Text = "Player go";
            steps = 0;
            go = true;
            isCircle = true;
            isFirst = true;
            the5 = false;
            CenterOrCorner = true;
            button1.Visible = false;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            for (int i = 0; i < 9; ++i) table[i] = 0;
            g1.DrawRectangles(new Pen(Color.Black, 5), rect);
        }

        // 按下狀態列的悔棋
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        // 按下視窗內的悔棋
        private void button2_Click(object sender, EventArgs e)
        {
            Undo();
        }

        // 悔棋
        private void Undo()
        {
            if (steps > 1 && !over(this.CreateGraphics())) // 至少要各下一步(總共2步)，而且遊戲還沒結束才能悔棋
            {
                Graphics g1 = this.CreateGraphics();
                g1.Clear(this.BackColor); // 清除所有視窗內容重畫

                table[StepsList[--steps]] = 0; // 清除最後電腦下的子
                table[StepsList[--steps]] = 0; // 清除最後玩家下的子
                g1.DrawRectangles(new Pen(Color.Black, 5), rect); // 重畫矩形

                int EmptyTotal = 0;
                for (int i = 0; i < 9; ++i) if (table[i] == 0) empty[EmptyTotal++] = i;

                for (int i = 0; i < 9; ++i) Draw_Picture(g1, i); // 重畫矩形狀態
                return;
            }
        }

    }
}