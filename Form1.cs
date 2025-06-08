using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace ImageToASCIIconverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string[] _AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", "&nbsp;" };
        private string _Content;
        
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        
        private void btnConvertToAscii_Click(object sender, EventArgs e)
        {
            btnConvertToAscii.Enabled = false;     
            //TODO Carga da imagem
            Bitmap image = new Bitmap(txtPath.Text, true);            
            //TODO Dimensiona a imagem ...

            image = GetReSizedImage(image,this.trackBar.Value);           

            //TODO Converte a imagem redimensionada em ASCII
            _Content = ConvertToAscii(image);

            //TODO Coloque a string final entre a tag <pre> para preservar sua formatação
            browserMain.DocumentText = "<pre>" + "<Font size=0>" + _Content + "</Font></pre>";               
            btnConvertToAscii.Enabled = true;
        }



        private string ConvertToAscii(Bitmap image)
        {
            Boolean toggle = false;
            StringBuilder sb = new StringBuilder();
            
            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    Color pixelColor = image.GetPixel(w, h);

                    //TODO Média dos componentes RGB para encontrar a cor cinza

                    int red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    Color grayColor = Color.FromArgb(red,green,blue);

                    //TODO Use o toggle para minimizar o alongamento em relação à altura

                    if (!toggle)
                    {
                        int index = (grayColor.R * 10) / 255;
                        sb.Append(_AsciiChars[index]);
                    }
                }
                if (!toggle)
                {
                    sb.Append("<BR>");
                    toggle = true;
                }
                else
                {
                    toggle = false;
                }
            }
           
            return sb.ToString();
        }


        private Bitmap GetReSizedImage(Bitmap inputBitmap, int asciiWidth )
        {            
            int asciiHeight=0;

            //TODO Calcula a nova Altura da imagem a partir de sua largura
            asciiHeight = (int)Math.Ceiling((double)inputBitmap.Height * asciiWidth / inputBitmap.Width);

            //TODO Cria um novo Bitmap e definir sua resolução
            Bitmap result = new Bitmap(asciiWidth, asciiHeight);
            Graphics g = Graphics.FromImage((Image)result);
            //TODO Interpolação para imagens de alta resolução 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
            g.Dispose();
            return result;
        }

     
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult diag = openFileDialog1.ShowDialog();
            if (diag == DialogResult.OK)
            {
                txtPath.Text = openFileDialog1.FileName;
            }
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text File (*.txt)|.txt|HTML (*.htm)|.htm";
            DialogResult diag = saveFileDialog1.ShowDialog();
            if (diag == DialogResult.OK)
            {
                if (saveFileDialog1.FilterIndex == 1)
                {
                    //TODO Se o formato a ser salvo for HTML substitiu todos os espaços HTML por espaços padrão e todas as quebras de linha para Carriage Return, LineFeed
                    _Content = _Content.Replace("&nbsp;", " ").Replace("<BR>","\r\n");
                }
                else
                {
                    //TODO Usar a tag <pre></pre> para preservar a formatação ao visualizá-la no navegador
                    _Content = "<pre>" + _Content + "</pre>";
                }
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                sw.Write(_Content);
                sw.Flush();
                sw.Close();
            }
        }

    }
}