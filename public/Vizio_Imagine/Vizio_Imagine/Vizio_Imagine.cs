/*Nota: Cuando programe esto, solo Dios y yo sabíamos cómo... ahora solo Dios sabe...*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Vizio_Imagine
{
    unsafe public partial class Vizio_Imagine : Form
    {
        private List<string> lista_rutas = new List<string>();
        public Vizio_Imagine()
        {
            InitializeComponent();
        }

        //Evento load, aquí se inicializa dando la ruta e iniciando el método "llenar_combobox"
        private void Vizio_Imagine_Load(object sender, EventArgs e) {
            string ruta = @"C:\Users\Toshiba\Documents\tiendacosmeticos\public\images";
            llenar_combobox(comboBox_vizio, Directory.GetFiles(ruta));
        }

        /*Método que se encarga de llenar el combobox con los nombres de los archivos
        ademas agrega las rutas a la lista "lista_rutas", también inicializa el combobox
        al primer elemento, o sea al elemento 0
        se añadio que el checkbox iniciara en true*/
        private void llenar_combobox(ComboBox caja, string[] rutas) {
            caja.Items.Clear();
            string archivos;

            foreach(string nombre in rutas) {
                archivos = Path.GetFileName(nombre);
                caja.Items.Add(archivos);
                lista_rutas.Add(nombre);
            }

            caja.SelectedIndex = 0;
            checkBox_normal.Checked = true;
            toolStrip_btn1.BackColor = Color.Orange;
        }

        /*Método que se encarga de mostrar la imagen en el PictureBox de manera predeterminada
        añadido: funciones para iterar entre los elementos visuales y editado de imagen*/
        private void mostrar_imagen(PictureBox galeria, string ruta) {

            if(radioButton_centrada.Checked) {
                galeria.SizeMode = PictureBoxSizeMode.CenterImage;
                toolStrip_btn4.BackColor = Color.White;
                toolStrip_btn5.BackColor = Color.White;
            } else if(radioButton_ajustar.Checked) {
                galeria.SizeMode = PictureBoxSizeMode.StretchImage;
                toolStrip_btn3.BackColor = Color.White;
                toolStrip_btn5.BackColor = Color.White;
            } else if(radioButton_zoom.Checked){
                galeria.SizeMode = PictureBoxSizeMode.Zoom;
                toolStrip_btn3.BackColor = Color.White;
                toolStrip_btn4.BackColor = Color.White;
            }

            if (checkBox_griss.Checked) {

                //Método lento
                /*Bitmap imagen_editar = (new Bitmap(galeria.Image = Image.FromFile(ruta)));

                for (int i = 0; i < imagen_editar.Width; i++) {
                    for (int j = 0; j < imagen_editar.Height; j++) {
                        Color x = (imagen_editar.GetPixel(i, j));
                        int colores = x.ToArgb();
                        int rojo = (int)((colores & 0x0000FF) * 0.3);
                        int verde = (int)(((colores & 0x00FF00) >> 8) * 0.59);
                        int azul = (int)(((colores & 0xFF0000) >> 16) * 0.11);
                        int gris = rojo + verde + azul;
                        imagen_editar.SetPixel(i, j, Color.FromArgb(gris | gris << 8 | gris << 16 | 255 << 24));
                    }
                    galeria.Image = imagen_editar;
                }*/

                //Llamada a un metodo para escala de grises mas rapido, pero en ciertos casos no funciona
                /*Bitmap imagen = new Bitmap(ruta);
                galeria.Image = crear_gris(imagen);*/

                //Método definitivo, algo difícil de entender, implica punteros, por lo que es necesario anteponer a la clase "unsafe"
                galeria.Image = Image.FromFile(ruta);
                galeria.Tag = ruta;
                Bitmap imagen_editar = (new Bitmap(galeria.Image));
                Rectangle rec = new Rectangle(0, 0, imagen_editar.Width, imagen_editar.Height);
                BitmapData bmpD = imagen_editar.LockBits(rec, ImageLockMode.ReadWrite, imagen_editar.PixelFormat);
                IntPtr ptr = bmpD.Scan0;
                byte* puntero = (byte*)ptr.ToPointer();
                int bytes = bmpD.Stride * imagen_editar.Height;
                for(int i = 3; i < bytes; i += 4) {
                    byte nuevo_valor = (byte)((int)((*(puntero + i - 3)) * .11) + (int)((*(puntero + i - 2)) * .59) + (int)((*(puntero + i - 1)) * .3));
                    puntero[i - 1] = puntero[i - 2] = puntero[i - 3] = nuevo_valor;
                }
                imagen_editar.UnlockBits(bmpD);
                galeria.Image = imagen_editar;

                toolStrip_btn1.BackColor = Color.White;
            } else {
                galeria.Image = Image.FromFile(ruta);
                galeria.Tag = ruta;
                toolStrip_btn2.BackColor = Color.White;
            }
        }

        /*Método que se encarga de que cuando el index en el combobox cambie, entonces se inicie
        este método, llamando al método "mostrar_imagen", además si el índice del combobox es 0,
        o sea la primera imagen, no puede haber desplazamiento hacia antes de eso, así que
        se encargara de bloquear los botones "<", "<<", lo mismo va para cuando se llega al
        final, solo que obtenemos el tamaño del arreglo y luego comparamos, se encarga de los
        botones ">" ">>"
        añadido: mostrar la ruta en la barra de estatus ubicada abajo.*/
        private void comboBox_vizio_SelectedIndexChanged(object sender, EventArgs e) {
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
            toolStripStatus_lb1.Text = lista_rutas[comboBox_vizio.SelectedIndex];

            if(comboBox_vizio.SelectedIndex == 0) {
                btn2_vizio.Enabled = false;
                btn1_vizio.Enabled = false;
            } else {
                btn2_vizio.Enabled = true;
                btn1_vizio.Enabled = true;
            }

            int cantidad = lista_rutas.Count;

            if(comboBox_vizio.SelectedIndex == cantidad-1) {
                btn3_vizio.Enabled = false;
                btn4_vizio.Enabled = false;
            } else {
                btn3_vizio.Enabled = true;
                btn4_vizio.Enabled = true;
            }
        }

        /*Metodo click para moverse entre las imagenes, pertence al boton con el simbolo "<<"
        primer elemento*/
        private void btn1_vizio_Click(object sender, EventArgs e) {
            comboBox_vizio.SelectedIndex = 0;
        }

        /*Metodo click para moverse entre las imagenes, pertence al boton con el simbolo "<"
        yendo hacia atras*/
        private void btn2_vizio_Click(object sender, EventArgs e) {
            comboBox_vizio.SelectedIndex -= 1;
        }

        /*Metodo click para moverse entre las imagenes, pertence al boton con el simbolo ">"
        yendo hacia adelante*/
        private void btn3_vizio_Click(object sender, EventArgs e) {
            comboBox_vizio.SelectedIndex += 1;
        }

        /*Metodo click para moverse entre las imagenes, pertence al boton con el simbolo ">>"
        ultimo elemento*/
        private void btn4_vizio_Click(object sender, EventArgs e) {
            int ultimo = lista_rutas.Count - 1;
            comboBox_vizio.SelectedIndex = ultimo;
        }

        //método que se encarga de iterar entre normal y grises, este es el de normal.
        private void checkBox_normal_Click(object sender, EventArgs e) {
            if(!checkBox_griss.Checked && !checkBox_normal.Checked) {
                checkBox_normal.Checked = true;
                return;
            }

            checkBox_griss.Checked = false;
            toolStrip_btn1.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void toolStrip_btn1_Click(object sender, EventArgs e) {
            if(toolStrip_btn1.BackColor == Color.Orange) {
                return;
            }

            checkBox_normal.Checked = true;
            checkBox_griss.Checked = false;
            toolStrip_btn1.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void subvis_normal_Click(object sender, EventArgs e) {
            if (toolStrip_btn1.BackColor == Color.Orange) {
                return;
            }

            checkBox_normal.Checked = true;
            checkBox_griss.Checked = false;
            toolStrip_btn1.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }


        //método que se encarga de iterar entre normal y grises, este es el de gris.
        /*Dato: Por alguna razon, si desabilito el check box de la escala de grises
        el programa explota, así que cambie la manera de iterar con ello*/
        private void checkBox_griss_Click(object sender, EventArgs e) {
            if (!checkBox_griss.Checked && !checkBox_normal.Checked) {
                checkBox_griss.Checked = true;
                return;
            }

            checkBox_normal.Checked = false;
            toolStrip_btn2.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void toolStrip_btn2_Click(object sender, EventArgs e) {
            if(toolStrip_btn2.BackColor == Color.Orange) {
                return;
            }

            checkBox_griss.Checked = true;
            checkBox_normal.Checked = false;
            toolStrip_btn2.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void subvis_esc_grises_Click(object sender, EventArgs e) {
            if (toolStrip_btn2.BackColor == Color.Orange) {
                return;
            }

            checkBox_griss.Checked = true;
            checkBox_normal.Checked = false;
            toolStrip_btn2.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        //eventos on click de centrado de imagenes
        private void toolStrip_btn3_Click(object sender, EventArgs e) {
            if(toolStrip_btn3.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn3.BackColor = Color.Orange;
            radioButton_centrada.Checked = true;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void radioButton_centrada_Click(object sender, EventArgs e) {
            if (toolStrip_btn3.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn3.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void subtam_centrada_Click(object sender, EventArgs e) {
            if (toolStrip_btn3.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn3.BackColor = Color.Orange;
            radioButton_centrada.Checked = true;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        //eventos on click de expansión de imagenes
        private void toolStrip_btn4_Click(object sender, EventArgs e) {
            if (toolStrip_btn4.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn4.BackColor = Color.Orange;
            radioButton_ajustar.Checked = true;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void radioButton_ajustar_Click(object sender, EventArgs e) {
            if (toolStrip_btn4.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn4.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void subtam_ajustar_Click(object sender, EventArgs e) {
            if (toolStrip_btn4.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn4.BackColor = Color.Orange;
            radioButton_ajustar.Checked = true;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        //eventos on click de expansión de imagenes
        private void toolStrip_btn5_Click(object sender, EventArgs e) {
            if (toolStrip_btn5.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn5.BackColor = Color.Orange;
            radioButton_zoom.Checked = true;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void radioButton_zoom_Click(object sender, EventArgs e) {
            if (toolStrip_btn5.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn5.BackColor = Color.Orange;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        private void subtam_zoom_Click(object sender, EventArgs e) {
            if (toolStrip_btn5.BackColor == Color.Orange) {
                return;
            }

            toolStrip_btn5.BackColor = Color.Orange;
            radioButton_zoom.Checked = true;
            mostrar_imagen(pictureBox_vizio, lista_rutas[comboBox_vizio.SelectedIndex]);
        }

        //método para guardar la imagen

        private void subarch_guardar_Click(object sender, EventArgs e) {
            string nombArchivo = Path.GetFileName(pictureBox_vizio.Tag.ToString());
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "JPEG(*.JPG)|*.JPG|BMP(*.BMP)|*.BMP";
            Image imagen = pictureBox_vizio.Image;
            guardar.Title = "Guardar imagen";
            guardar.FileName = nombArchivo;
            guardar.ShowDialog();

            if(guardar.FileName == "") {
                return;
            }

            imagen.Save(guardar.FileName);
        }

        //método para  salir de la aplicación
        private void subarch_salir_Click(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        //Métodos del ContextMenuStrip para girar y copiar la imagen
        private void girar90ALaIzquierdaToolStripMenuItem_Click(object sender, EventArgs e) {
            Image imagen = pictureBox_vizio.Image;
            imagen.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBox_vizio.Image = imagen;
        }

        private void girar90ALaDerechaToolStripMenuItem_Click(object sender, EventArgs e) {
            Image imagen = pictureBox_vizio.Image;
            imagen.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox_vizio.Image = imagen;
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e) {
            Image imagen = pictureBox_vizio.Image;
            Clipboard.Clear();
            Clipboard.SetImage(imagen);
        }

        //métodos necesarios para convertit la imagen a escala de grises, no se uso por problemas

        /*private byte[] obtener_img_bytes(Bitmap imagen, ImageLockMode lockMode, out BitmapData pixeles_info) {
            pixeles_info = imagen.LockBits(new Rectangle(0, 0, imagen.Width, imagen.Height), lockMode, imagen.PixelFormat);
            byte[] bytes_imagen = new byte[pixeles_info.Stride * imagen.Height];
            Marshal.Copy(pixeles_info.Scan0, bytes_imagen, 0, bytes_imagen.Length);

            return bytes_imagen;
        }

        private Bitmap crear_gris(Bitmap recurso) {
            Bitmap objetivo = new Bitmap(recurso.Width, recurso.Height, recurso.PixelFormat);
            BitmapData info_objetivo, info_recurso;

            byte[] recurso_bytes = obtener_img_bytes(recurso, ImageLockMode.ReadOnly, out info_recurso);
            byte[] objetivo_bytes = obtener_img_bytes(objetivo, ImageLockMode.ReadWrite, out info_objetivo);

            for(int i = 0; i < recurso_bytes.Length; i += 3) {
                if((i + 3) % (recurso.Width * 3) > 0) {
                    byte y = (byte)(recurso_bytes[i + 2] * 0.3f + recurso_bytes[i + 1] * 0.59f + recurso_bytes[i] * 0.11f);
                    objetivo_bytes[i + 2] = objetivo_bytes[i + 1] = objetivo_bytes[i] = y;
                }
            }

            Marshal.Copy(objetivo_bytes, 0, info_objetivo.Scan0, objetivo_bytes.Length);

            recurso.UnlockBits(info_recurso);
            objetivo.UnlockBits(info_objetivo);

            return objetivo;
        }*/

    }
}
