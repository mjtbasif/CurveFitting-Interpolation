using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace numeric
{
    public partial class Form2 : Form
    {
        double a, b;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.Location = this.Location;
            f1.Show();
        }

        private void eqnCurve(object sender, EventArgs e) // calcuate curve fitting eqn
        {
            double sX, sY, sXY, sXX, temp;
            string xValues = textBox2.Text + " ";
            string yValues = textBox3.Text + " ";
            if (xValues == " " || yValues == " " || textBox1.Text == "") return;
            int N = int.Parse(textBox1.Text, System.Globalization.CultureInfo.InvariantCulture);
            sX = sY = sXY = sXX = 0;
            double[] X = new double[N + 10];
            int index = 0, f = 1 , unit=1;
            temp = 0;
            for(int i=0;i<xValues.Length;i++)
            {
                if(xValues[i]==' ')
                {
                    X[index++] = temp;
                    sX += temp;
                    sXX += (temp * temp);
                    temp = 0;
                    f = 1;
                    unit = 1;
                }
                else
                {
                    temp=(temp*unit)+(xValues[i] - '0');
                    if(f==1)
                    {
                        unit = 10;
                        f = 0;
                    }
                }
            }
            double[] Y = new double[N + 10];
            index = 0; f = 1; unit = 1; temp = 0;
            for (int i = 0; i < yValues.Length; i++)
            {
                if (yValues[i] == ' ')
                {
                    Y[index++] = temp;
                    sY += temp;
                    temp = 0;
                    f = 1;
                    unit = 1;
                }
                else
                {
                    temp = (temp * unit) + (yValues[i] - '0');
                    if (f == 1)
                    {
                        unit = 10;
                        f = 0;
                    }
                }
            }
            for (int i = 0; i < N; i++)
            {
                sXY += (X[i] * Y[i]);
            }
            b = (((N * sXY) - (sX * sY)) / ((N * sXX) - (sX * sX)));
            a = (sY / N) - (b * (sX / N));
            curveEqn.Text ="Y="+ string.Format("{0:F2}", b) + "X+"+ string.Format("{0:F2}", a);
        }
        double fact(int n)
        {
            double res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
            return res;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.Location = this.Location;
            f2.Show();
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            linearCurve.ForeColor = Color.GreenYellow;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            linearCurve.ForeColor = Color.Turquoise;
        }

        private void label7_MouseHover(object sender, EventArgs e)
        {
            calculateInterpolation.ForeColor = Color.GreenYellow;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            calculateInterpolation.ForeColor = Color.Turquoise;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user_1_;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user;
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.bar_chart_1_;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.bar_chart;
        }
        int mouseX = 0, mouseY = 0;
        bool mouseDown;

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.SetDesktopLocation(MousePosition.X- mouseX, MousePosition.Y- mouseY);
            }
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mouseX = e.X;
            mouseY=e.Y;
        }

        private void gaussBackward(object sender, EventArgs e) // calculate interpolation
        {
            if (textBox6.Text== "" || textBox4.Text == "" || textBox5.Text == "") return;
            double temp;
            string xValues = textBox5.Text + " ";
            int N = int.Parse(textBox4.Text, System.Globalization.CultureInfo.InvariantCulture);
            double x = double.Parse(textBox6.Text, System.Globalization.CultureInfo.InvariantCulture);
            
            double[] X = new double[N + 10];
            double[] Y = new double[N + 10];
            int index = 0, f = 1, unit = 1;
            temp = 0;
            for (int i = 0; i < xValues.Length; i++)
            {
                if (xValues[i] == ' ')
                {
                    X[index] = temp;
                    Y[index] = ((b * temp) + a);
                    index++;
                    temp = 0;
                    f = 1;
                    unit = 1;
                }
                else
                {
                    temp = (temp * unit) + (xValues[i] - '0');
                    if (f == 1)
                    {
                        unit = 10;
                        f = 0;
                    }
                }
            }
            //craeete table 
            double[,] y = new double[N+10, 1000];
            int[] sizeIN = new int[N+10];
            int IN = 0;
            for (int i = 0; i < N - 1; i++)
            {
                y[0, i] = Y[i + 1] - Y[i];
            }
            sizeIN[IN++] = N - 1;

            int n = N;
            n--;
            for (int i = 1; i < N - 1; i++)
            {
                //sizeIN[IN++] = n-1;
                for (int j = 0; j < n; j++)
                {
                    y[i, j] = y[i - 1, j + 1] - y[i - 1, j];
                }
                n--;
                sizeIN[IN++] = n;
            }
            // interpolation
            if ((x >= X[0] && x <= X[1]) || (x>=X[N-2]))
            {
                interRes.Text = "Impossible to use Gausse Backward Interpolation";
                return;
            }
            for(int i=0;i<N;i++)
            {
                if(x<X[i])
                {
                    interRes.Text = "Possible";
                    double x0 = X[i];
                    double y0 = Y[i];
                    double h = X[1] - X[0];
                    double u = (x - x0) / h;
                    index= (i + i - 1) / 2;
                    double res = y0 + (u * y[0,index]); //starting calculation
                    temp = u;
                    double val; //handy variables
                    int c = 1;  f = 0; // column count and flag
                    for(int j=1;j<N-1;j++)
                    {
                        if(j%2==0)
                        {
                            temp *= (u - ((j + 1) / 2));
                            c++;
                            index--;
                        }
                        else
                        {
                            temp *= (u + ((j + 1) / 2));
                            if(j>1)
                            {
                                c++;
                            }
                        }
                        if (sizeIN[c] < index) { val = 0; f = 1; }
                        else if (x > X[1] && x < X[2]) val = 0;
                        else val = y[c, index];

                        res += (temp / fact(j + 1)) * (val);
                        if (f == 1) break;
                    }
                    interRes.Text = string.Format("{0:F2}", res);
                    return;
                }
            }
        }
    }
}
