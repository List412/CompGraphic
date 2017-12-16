using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenGL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int WINDOW_BASE_HEIGHT = 600;
        const int WINDOW_BASE_WIDTH = 600;

        const int ROOM_BASE_LENGTH = 400;
        static bool direction = false;


        static int sphereAngle = 0;

        private static GameWindow game;

        public MainWindow()
        {
            InitializeComponent();
            Main1();
            this.Close();
        }

        private static int current_texture;

        private static Bitmap texture_source;
        private static int texture;

        public static float MoveLight(ref float x)
        {
            if (direction)
            {
                x += 0.18f;
            }
            else x -= 0.18f;

            return x;
        }

        public static void Main1()
        {
            game = new GameWindow(WINDOW_BASE_WIDTH, WINDOW_BASE_HEIGHT, new GraphicsMode(32, 4, 0, 2));

            float size = 50;
            float ligthX = size / 2f, ligthY = size - size / 10f, lightZ = size / 2f;
            float ligthTopX = size / 2f, ligthTopY = size - size / 10f, lightTopZ = size / 2f;
            int count = 15;
            int earthTexture = 1;
            int caverTexture = 1;
            int mishaTextures = 1;
            int footBallTextur = 1;
            game.Load += (sender, e) =>
            {
                // setup settings, load textures, sounds
                game.VSync = VSyncMode.On;

                GL.ClearColor(Color.SkyBlue);
                GL.Enable(EnableCap.DepthTest);

                Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(45 * game.Width / game.Height * Math.PI / 180), (float)game.Width / game.Height, 1, 500);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref p);

                Matrix4 modelview = Matrix4.LookAt(size / 2, size / 2, size - size / 10, size / 2, size / 2, 0, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);

                //var texture_source = new Bitmap("texture.png");


                ////Generate empty texture
                //texture = GL.GenTexture();

                ////Link empty texture to texture2d
                //GL.BindTexture(TextureTarget.Texture2D, texture);

                ////Must be set else the texture will show glColor
                ////GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                ////GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

                ////Describe to gl what we want the bound texture to look like
                //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture_source.Width, texture_source.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

                ////Lock pixel data to memory and prepare for pass through
                //System.Drawing.Imaging.BitmapData bitmap_data = texture_source.LockBits(new System.Drawing.Rectangle(0, 0, texture_source.Width, texture_source.Height),
                //    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                ////Tell gl to write the data from are bitmap image/data to the bound texture
                //GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, texture_source.Width, texture_source.Height, PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);

                ////Release from memory
                //texture_source.UnlockBits(bitmap_data);

                ////Release texture
                //GL.BindTexture(TextureTarget.Texture2D, 0);

                ////Enable textures from texture2d target
                //GL.Enable(EnableCap.Texture2D);
                earthTexture = LoadTexture("texture.png", 1, flip_y: true);
                current_texture = LoadTexture("grass.jpg", 1);
                caverTexture = LoadTexture("caver.jpg", 1);
                mishaTextures = LoadTexture("Misha.jpg", 1);
                footBallTextur = LoadTexture("football.png", 1);

                //Basically enables the alpha channel to be used in the color buffer
                GL.Enable(EnableCap.Blend);
                //The operation/order to blend
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                //Use for pixel depth comparing before storing in the depth buffer
                GL.Enable(EnableCap.DepthTest);               
            };

            game.Resize += (sender, e) =>
            {
                GL.Viewport(0, 0, game.Width, game.Height);

                Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(45 * game.Width / game.Height * Math.PI / 180), (float)game.Width / game.Height, 1, 500);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref p);

            };

            game.UpdateFrame += (sender, e) =>
            {
                // add game logic, input handling
                if (game.Keyboard[Key.Escape])
                {
                    game.Exit();
                }

                if (game.Keyboard[Key.Space])
                {

                    Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(45 * game.Width / game.Height * Math.PI / 180), (float)game.Width / game.Height, 1, 500);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref p);

                    Matrix4 modelview = Matrix4.LookAt(size / 2, size / 2, size - size / 10, size / 2, size / 2, 0, 0, 1, 0);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref modelview);

                }

                if (game.Keyboard[Key.Enter])
                {
                    game.WindowState = OpenTK.WindowState.Fullscreen;
                }

                if (game.Keyboard[Key.R])
                {
                    ligthX += 0.1f;
                    ligthY += 0.1f;
                    lightZ += 0.1f;
                }

                if (game.Keyboard[Key.T])
                {
                    ligthX -= 0.1f;
                    ligthY -= 0.1f;
                    lightZ -= 0.1f;
                }

                if (game.Keyboard[Key.Down])
                {
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.Translate(0, 0, -1);
                }

                if (game.Keyboard[Key.Up])
                {
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.Translate(0, 0, 1);
                }

                if (game.Keyboard[Key.Left])
                {
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.Translate(-1, 0, 0);
                }

                if (game.Keyboard[Key.Right])
                {
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.Translate(1, 0, 0);
                }

                if (game.Keyboard[Key.D])
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.Rotate(1, 0, 1, 0);
                }

                if (game.Keyboard[Key.A])
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.Rotate(-1, 0, 1, 0);
                }

                //123

                if (game.Keyboard[Key.S])
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.Rotate(1, 1, 0, 0);
                }

                if (game.Keyboard[Key.W])
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.Rotate(-1, 1, 0, 0);
                }

                if (game.Keyboard[Key.Q])
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.Rotate(1, 0, 0, 1);
                }

                if (game.Keyboard[Key.E])
                {
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.Rotate(-1, 0, 0, 1);
                }


            };

            game.RenderFrame += (sender, e) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                
                //light


                float[] mat_diffuse = { 0.1f, 0.1f, 0.1f };
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, mat_diffuse);

                float[] whiteSpecularLight = new float[] { 0.2f, 0.8f, 0.2f };
                float[] blackAmbientLight = new float[] { 0.2f, 0.2f, 0.2f };
                float[] whiteDiffuseLight = new float[] { 0.8f, 0.2f, 0.8f };

                GL.Light(LightName.Light0, LightParameter.Specular, whiteSpecularLight);
                GL.Light(LightName.Light0, LightParameter.Ambient, blackAmbientLight);
                GL.Light(LightName.Light0, LightParameter.Diffuse, whiteDiffuseLight);

                //GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 0.8f);
                //GL.Light(LightName.Light0, LightParameter.SpotDirection, new float[] { 0f, -1f, 0f });
                //GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 2f);
                //GL.Light(LightName.Light0, LightParameter.SpotCutoff, 34f);
                //GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 1.81f);
                //GL.Light(LightName.Light0, LightParameter.SpotExponent, 1f);
                GL.Light(LightName.Light0, LightParameter.Position, new float[] { ligthX, ligthY, lightZ, 1f });

                GL.ShadeModel(ShadingModel.Smooth);
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Light0);
                GL.Enable(EnableCap.ColorMaterial);

                Color[] colors = new Color[6];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = Color.White;
                }
                //DrawCube(0, 2, 0.01f, 2, ligthX - 1f, ligthY + 2f, lightZ - 1f, 15, colors);
                GL.Color3(Color.Black);
                DrawLine(ligthTopX, size, lightTopZ, ligthX, ligthY, lightZ);
                GL.Color3(Color.LightGoldenrodYellow);

                Sphere(1, 20, 20, ligthX, ligthY, lightZ, 0, true);

                count++;
                if (count > 20)
                {
                    direction = !direction;
                    count = 0;
                }
                GL.Disable(EnableCap.Texture2D);
                colors = new Color[]
                {
                    Color.Transparent, Color.Orange, Color.Brown, Color.DarkSlateBlue, Color.Sienna, Color.Orange
                };

                float k = 12f;
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, earthTexture);
                
                DrawCube(0, size, size, size*3, 0, 0, 0, 100, colors, false);
                GL.Disable(EnableCap.Texture2D);
               
                DrawCube(0, size, size / 25f, size / 10, 0, size - size / 5, 0, 80, colors);
               
                //axes
                GL.Color3(Color.Red);
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex3(0, 0, 0);
                GL.Vertex3(20, 0, 0);
                GL.Color3(Color.Yellow);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 40, 0);
                GL.Color3(Color.Green);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, 60);
                GL.End();

                k = 10;

                GL.Enable(EnableCap.Texture2D);
                GL.Color3(Color.Transparent);
                GL.BindTexture(TextureTarget.Texture2D, current_texture);
                Sphere(size / k, 90, 90, size / k + 2, size / k, size / k, 0);
                GL.BindTexture(TextureTarget.Texture2D, earthTexture);
                Sphere(size / k, 90, 90, size - size / k, size / k, size / k + 2, 0);
                GL.Disable(EnableCap.Texture2D);
                
                //sphereAngle++;

                for (float x = size / 5f; x < size; x += size / 5f)
                {
                    for (float y = size / 5f; y < size; y += size / 5f)
                    {
                        for (float z = size / 5f; z < size; z += size / 5f)
                        {
                            //Sphere(2, 25, 25, x, y, z, sphereAngle);
                        }
                    }
                }

                //DrawCube(30, 10, 10, 10, 30, 30, 30, 10, colors);

                GL.Disable(EnableCap.Texture2D);
                GL.Enable(EnableCap.AlphaTest);
                GL.Enable(EnableCap.Blend);
                //GL.DepthMask(false);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, footBallTextur);
                Sphere(size / 12, 60, 60, size / 3 - size / 12, size / 12, size / 2, 0);
                //GL.DepthMask(true);
                GL.Disable(EnableCap.AlphaTest);
                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.Texture2D);
                // GL.Translate(size / 2, size / 2, 0);
                game.SwapBuffers();
            };

            // Run the game at 60 updates per second
            game.Run(60f);

        }

        static void DrawLine(float fromX, float flomY, float fromZ, float toX, float toY, float toZ)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(fromX, flomY, fromZ);
            GL.Vertex3(toX, toY, toZ);
            GL.End();
        }

        static void Room(float width)
        {
            /*задняя*/
            GL.Normal3(0, 0, width);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*левая*/
            GL.Normal3(width, 0, 0);
            GL.Color3(Color.Orange);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*нижняя*/
            GL.Normal3(0, width, 0);
            GL.Color3(Color.Coral);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, 0, 0);
            GL.End();

            /*верхняя*/
            GL.Normal3(0, -width, 0);
            GL.Color3(Color.RoyalBlue);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(0, width, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*правая*/
            GL.Normal3(-width, 0, 0);
            GL.Color3(Color.Orange);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*передняя*/
            GL.Normal3(0, 0, -width);
            GL.Color3(Color.Sienna);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();
        }

        static void Cubek(float width)
        {
            /*задняя*/
            GL.Normal3(0, 0, -width);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*левая*/
            GL.Normal3(-width, 0, 0);
            GL.Color3(Color.Orange);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*нижняя*/
            GL.Normal3(0, -width, 0);
            GL.Color3(Color.Coral);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, 0, 0);
            GL.End();

            /*верхняя*/
            GL.Normal3(0, width, 0);
            GL.Color3(Color.RoyalBlue);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(0, width, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*правая*/
            GL.Normal3(width, 0, 0);
            GL.Color3(Color.Orange);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*передняя*/
            GL.Normal3(0, 0, width);
            GL.Color3(Color.Sienna);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();


            /*ребра*/
            /*
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(width, 0, 0);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();
            */
        }

        static void DrawCube(float angle, float width, float height, float depth, float positionX, float positionY, float positionZ, float polygons, Color[] colors6, bool outerLight = true)
        {            

            float l = outerLight ? -1 : 1;

            float stepX = width / polygons;
            float stepY = height / polygons;
            float stepZ = depth / polygons;

            /*задняя*/
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(colors6[0]);
            GL.Normal3(0f, 0f, 1f * l);
            for (float w = 0; w < width; w += stepX)
            {
                for (float h = 0; h < height; h += stepY)
                {
                    GL.TexCoord2(w / width, h / height);
                    GL.Vertex3(positionX + w, positionY + h, positionZ);

                    GL.TexCoord2(w / width + 1.0f / polygons, h / height);
                    GL.Vertex3(positionX + w + stepX, positionY + h, positionZ);

                    GL.TexCoord2(w / width + 1.0f / polygons, h / height + 1.0f / polygons);
                    GL.Vertex3(positionX + w + stepX, positionY + h + stepY, positionZ);

                    GL.TexCoord2(w / width, h / height + 1.0f / polygons);
                    GL.Vertex3(positionX + w, positionY + h + stepY, positionZ);
                }
            }
            GL.End();

            /*левая*/
            GL.Normal3(1f * l, 0f, 0f);
            GL.Color3(colors6[1]);
            GL.Begin(PrimitiveType.Polygon);

            for (float d = 0; d < depth; d += stepZ)
            {
                for (float h = 0; h < height; h += stepY)
                {                                                                
                    GL.Vertex3(positionX, positionY + h, positionZ + d);                   
                    GL.Vertex3(positionX, positionY + h + stepY, positionZ + d);                  
                    GL.Vertex3(positionX, positionY + h + stepY, positionZ + d + stepZ);                  
                    GL.Vertex3(positionX, positionY + h, positionZ + d + stepZ);
                }
            }
            GL.End();

            /*нижняя*/
            GL.Normal3(0f, 1f * l, 0f);
            GL.Color3(colors6[2]);
            GL.Begin(PrimitiveType.Polygon);
            for (float d = 0; d < depth; d += stepZ)
            {
                for (float w = 0; w < width; w += stepX)
                {
                    GL.Vertex3(positionX + w, positionY, positionZ + d);
                    GL.Vertex3(positionX + w + stepX, positionY, positionZ + d);
                    GL.Vertex3(positionX + w + stepX, positionY, positionZ + d + stepZ);
                    GL.Vertex3(positionX + w, positionY, positionZ + d + stepZ);
                }
            }
            GL.End();

            /*верхняя*/
            GL.Normal3(0f, -1f * l, 0f);
            GL.Color3(colors6[3]);
            GL.Begin(PrimitiveType.Polygon);
            for (float d = 0; d < depth; d += stepZ)
            {
                for (float w = 0; w < width; w += stepX)
                {
                    GL.Vertex3(positionX + w, positionY + height, positionZ + d);
                    GL.Vertex3(positionX + w + stepX, positionY + height, positionZ + d);
                    GL.Vertex3(positionX + w + stepX, positionY + height, positionZ + d + stepZ);
                    GL.Vertex3(positionX + w, positionY + height, positionZ + d + stepZ);
                }
            }
            GL.End();

            /*передняя*/
            GL.Normal3(0f, 0f, -1f * l);
            GL.Color3(colors6[4]);
            GL.Begin(PrimitiveType.Polygon);
            for (float w = 0; w < width; w += stepX)
            {
                for (float h = 0; h < height; h += stepY)
                {                                                                                              
                    GL.TexCoord2(w / width, h / height);
                    GL.Vertex3(positionX + w, positionY + h, positionZ + depth);
                    GL.TexCoord2(w / width + 1.0f / polygons, h / height);
                    GL.Vertex3(positionX + w + stepX, positionY + h, positionZ + depth);
                    GL.TexCoord2(w / width + 1.0f / polygons, h / height + 1.0f / polygons);
                    GL.Vertex3(positionX + w + stepX, positionY + h + stepY, positionZ + depth);
                    GL.TexCoord2(w / width, h / height + 1.0f / polygons);
                    GL.Vertex3(positionX + w, positionY + h + stepY, positionZ + depth);
                }
            }
            GL.End();

            /*правая*/
            GL.Normal3(-1f * l, 0f, 0f);
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(colors6[5]);
            for (float d = 0; d < depth; d += stepZ)
            {
                for (float h = 0; h < height; h += stepY)
                {
                    GL.Vertex3(positionX + width, positionY + h, positionZ + d);
                    GL.Vertex3(positionX + width, positionY + h + stepY, positionZ + d);
                    GL.Vertex3(positionX + width, positionY + h + stepY, positionZ + d + stepZ);
                    GL.Vertex3(positionX + width, positionY + h, positionZ + d + stepZ);
                }
            }
            GL.End();
            
        }



        private static int LoadTexture(string path, int quality = 0, bool repeat = true, bool flip_y = false)
        {
            Bitmap bitmap = new Bitmap(path);

            //Flip the image
            if (flip_y)
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            //Generate a new texture target in gl
            int texture = GL.GenTexture();

            //Will bind the texture newly/empty created with GL.GenTexture
            //All gl texture methods targeting Texture2D will relate to this texture
            GL.BindTexture(TextureTarget.Texture2D, texture);

            //The reason why your texture will show up glColor without setting these parameters is actually
            //TextureMinFilters fault as its default is NearestMipmapLinear but we have not established mipmapping
            //We are only using one texture at the moment since mipmapping is a collection of textures pre filtered
            //I'm assuming it stops after not having a collection to check.
            switch (quality)
            {
                case 0:
                default://Low quality
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                    break;
                case 1://High quality
                       //This is in my opinion the best since it doesnt average the result and not blurred to shit
                       //but most consider this low quality...
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                    break;
            }

            if (repeat)
            {
                //This will repeat the texture past its bounds set by TexImage2D
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
            }
            else
            {
                //This will clamp the texture to the edge, so manipulation will result in skewing
                //It can also be useful for getting rid of repeating texture bits at the borders
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
            }

            //Creates a definition of a texture object in opengl
            /* Parameters
             * Target - Since we are using a 2D image we specify the target Texture2D
             * MipMap Count / LOD - 0 as we are not using mipmapping at the moment
             * InternalFormat - The format of the gl texture, Rgba is a base format it works all around
             * Width;
             * Height;
             * Border - must be 0;
             * 
             * Format - this is the images format not gl's the format Bgra i believe is only language specific
             *          C# uses little-endian so you have ARGB on the image A 24 R 16 G 8 B, B is the lowest
             *          So it gets counted first, as with a language like Java it would be PixelFormat.Rgba
             *          since Java is big-endian default meaning A is counted first.
             *          but i could be wrong here it could be cpu specific :P
             *          
             * PixelType - The type we are using, eh in short UnsignedByte will just fill each 8 bit till the pixelformat is full
             *             (don't quote me on that...)
             *             you can be more specific and say for are RGBA to little-endian BGRA -> PixelType.UnsignedInt8888Reversed
             *             this will mimic are 32bit uint in little-endian.
             *             
             * Data - No data at the moment it will be written with TexSubImage2D
             */
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            //Load the data from are loaded image into virtual memory so it can be read at runtime
            System.Drawing.Imaging.BitmapData bitmap_data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Writes data to are texture target
            /* Target;
             * MipMap;
             * X Offset - Offset of the data on the x axis
             * Y Offset - Offset of the data on the y axis
             * Width;
             * Height;
             * Format;
             * Type;
             * Data - Now we have data from the loaded bitmap image we can load it into are texture data
             */
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);

            //Release from memory
            bitmap.UnlockBits(bitmap_data);

            //get rid of bitmap object its no longer needed in this method
            bitmap.Dispose();

            /*Binding to 0 is telling gl to use the default or null texture target
            *This is useful to remember as you may forget that a texture is targeted
            *And may overflow to functions that you dont necessarily want to
            *Say you bind a texture
            *
            * Bind(Texture);
            * DrawObject1();
            *                <-- Insert Bind(NewTexture) or Bind(0)
            * DrawObject2();
            * 
            * Object2 will use Texture if not set to 0 or another.
            */
            GL.BindTexture(TextureTarget.Texture2D, 0);

            return texture;
        }

        static void Sphere(double radius, int xPoligonCount, int yPoligonCount, double xOffset, double yOffset, double zOffset, float angle, bool insideL = false)
        {           

            //GL.Translate(xOffset + radius, yOffset + radius, zOffset + radius);
            //GL.Rotate(angle, 0, 1, 0);

            float lightDir = insideL ? -1 : 1;

            var stepX = 1.0 / (double)xPoligonCount;
            var stepY = 1.0 / (double)yPoligonCount;
            // Рисуем полигональную модель сферы, формируем нормали и задаем коодинаты текстуры
            // Каждый полигон - это трапеция. Трапеции верхнего и нижнего слоев вырождаются в треугольники
            GL.Begin(PrimitiveType.QuadStrip);
            var piy = Math.PI * stepY;
            var pix = Math.PI * stepX;
            for (int j = 0; j < yPoligonCount; j++)
            {
                var ay = j * piy;
                var sinY = Math.Sin(ay);
                var cosY = Math.Cos(ay);
                var texCoordY = j * stepY;
                var ayNext = ay + piy;
                var sinYNext = Math.Sin(ayNext);
                var cosYNext = Math.Cos(ayNext);
                var texCoordYNext = texCoordY + stepY;
                for (int i = 0; i <= xPoligonCount; i++)
                {
                    var ax = 2.0 * i * pix;
                    var sinX = Math.Sin(ax);
                    var cosX = Math.Cos(ax);
                    var x = radius * sinY * cosX;
                    var y = radius * sinY * sinX;
                    var z = radius * cosY;
                    var texCoordX = (double)i * stepX;
                    // Координаты нормали в текущей вершине
                    GL.Normal3(x * lightDir, y * lightDir, z * lightDir); // Нормаль направлена от центра
                                                                          // Координаты текстуры в текущей вершине
                    GL.TexCoord2(texCoordX, texCoordY);
                    GL.Vertex3(x + xOffset, y + yOffset, z + zOffset);

                    x = radius * sinYNext * cosX;
                    y = radius * sinYNext * sinX;
                    z = radius * cosYNext;
                    GL.Normal3(x * lightDir, y * lightDir, z * lightDir);
                    GL.TexCoord2(texCoordX, texCoordYNext);
                    GL.Vertex3(x + xOffset, y + yOffset, z + zOffset);
                }
            }
            GL.End();
            //GL.Rotate(angle, 0, -1, 0);
            //GL.Translate(-(xOffset + radius), -(yOffset + radius), -(zOffset + radius));
          
        }


        Vector3 crossProduct(Vector3 v1, Vector3 v2)
        {
            Vector3 cross = new Vector3(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X);
            return cross;
        }

        Vector3 getSurfaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {

            /*
            * obtain vectors between the coordinates of
            * triangle.
            */
            Vector3 polyVector1 = new Vector3(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z);
            Vector3 polyVector2 = new Vector3(v3.X - v1.X, v3.Y - v1.Y, v3.Z - v1.Z);

            Vector3 cross = crossProduct(polyVector1, polyVector2);

            Vector3.Normalize(cross);

            return cross;

        }

    }
}
