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
            f1.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e) // calcuate curve fitting eqn
        {
            double sX, sY, sXY, sXX, temp;
            string xValues = textBox2.Text + " ";
            string yValues = textBox3.Text + " ";
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
        private void label7_Click(object sender, EventArgs e) // calculate interpolation
        {
            double temp;
            string xValues = textBox5.Text + " ";
            int N = int.Parse(textBox4.Text, System.Globalization.CultureInfo.InvariantCulture);
            int x = int.Parse(textBox6.Text, System.Globalization.CultureInfo.InvariantCulture);
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
            if ((x >= X[0] && x <= X[1]) || (x>=X[N-2] && x<=X[N-1]))
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
                        if (sizeIN[c] ==index) { val = 0; f = 1; }
                        else val = y[c,index];
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
