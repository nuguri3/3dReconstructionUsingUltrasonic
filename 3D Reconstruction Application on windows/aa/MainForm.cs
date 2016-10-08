
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using Tao.Platform.Windows;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Sdl;
using Tao.DevIl;

namespace aa
{
    public partial class Betman : Form
    {

        public Betman()
        {
            
            a = b = c = cnt = 0;
            InitializeComponent();
            simpleOpenGlControl.InitializeContexts();
            auto_Rbt.Checked = true; //초기에 autumatic 버튼 선택

            for (int h = 0; h < 24; h++)
                distance[h] = 0;

            //SerialPortInitialize();
            //Initialize_coordinates();  
        }

        public static void Main(string[] args)
        { 
            Application.Run(new Betman());
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SerialPortInitialize(); //시리얼 통신 초기화

            DrawInitialize();
            // Draw(0.0f);
            Draw_Texture_Init();
            LoadGLTextures();

            
            Glu.gluLookAt(0.0, 0.0, 20.0, 0.0, 0.0, 0.0, 0.0, 5.0, 0.0); //카메라 시점 조정


            //Draw_Grid();
            Draw_Texture_Scene(-20.0f, -7.0f, -20.0f); //3D 환경 복원


         
           
        }

        //변수 및 좌표저장 구조

        private static int[] texture = new int[1];
        private static int[] texture1 = new int[1];
        private int []distance = new int[24];
        private int distance_count = 0;
        private static Glu.GLUquadric q;   
        private int[] index;
        private int width;
        private int height;
        private static float xrot = 10.0f; //회전 변수
        private static float yrot = 80.0f;
        private static float zrot = 20.0f;
        private int noOfPoint = 14; //포인트의 개수
        private static float a, b, c;
        private int[] x, y, z;
        private int cnt;
        private int xAccelmeter, yAccelmeter;
        private int mode; //0번 : 이동 1번 : 센서값 수신

        public struct realPoint
        {
            public float x, y, z;
            public int id;
        };

        realPoint[] p_array = new realPoint[20]; //원점 기준으로 좌표 변환된 포인트들 저장

        public struct coordinates
        {
            public int no;
            public int x;
            public int y;
            public int z;

            public coordinates(int no, int x, int y, int z)
            {
                this.no = no;
                this.x = x;
                this.y = y;
                this.z = z;

            }
        }

        public coordinates []coordSet = new coordinates[24] //좌표 저장 변수 배열
         {
             new coordinates(1,-5,-8,5),
             new coordinates(2,-5,8,5),
             new coordinates(3,10,-8,-20),
             new coordinates(4,15,8,-40),
             new coordinates(5,30,-8,-30),
             new coordinates(6,30,8,-30),
             new coordinates(7,35,-8,-60),
             new coordinates(8,35,8,-60),
             new coordinates(9,55,-8,-60),
             new coordinates(10,55,8,-60),
             new coordinates(11,60,-8,-40),
             new coordinates(12,62,8,-35),
             new coordinates(13,67,-8,-30),
             new coordinates(14,67,8,-35),
             new coordinates(12,62,8,-35),
             new coordinates(13,67,-8,-30),
             new coordinates(14,67,8,-35),
             new coordinates(12,62,8,-35),
             new coordinates(13,67,-8,-30),
             new coordinates(14,67,8,-35),
             new coordinates(12,62,8,-35),
             new coordinates(13,67,-8,-30),
             new coordinates(14,67,8,-35),
             new coordinates(14,67,8,-35)
        };

        private void Set_Coordinate(int index)
        {
            //평면의 방정식을 얻기 위해서는 3D 포인트 3개가 필요
            //시작점(빨간공)기준으로 좌표점 정렬 원점과 (-20,20,-5) 떨어져 있음.
            p_array[index].x = coordSet[index].x - 20.0f;
            p_array[index].y = coordSet[index].y + 20.0f;
            p_array[index].z = coordSet[index].z - 5.0f;

            p_array[index].x = coordSet[index + 1].x - 20.0f;
            p_array[index].y = coordSet[index + 1].y + 20.0f;
            p_array[index].z = coordSet[index + 1].z - 5.0f;

            p_array[index].x = coordSet[index + 2].x - 20.0f;
            p_array[index].y = coordSet[index + 2].y + 20.0f;
            p_array[index].z = coordSet[index + 2].z - 5.0f;

        }






        public void Delay(int ms) //딜레이 함수
        {
            int time = Environment.TickCount;

            do
            {
                if (Environment.TickCount - time >= ms)
                    return;
            } while (true);

        }



        //Draw 관련 함수들

        private void DrawInitialize()
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);
            Gl.glClearColor(0, 0, 0, 1); //화면을 특정색깔로 채운다

            Gl.glViewport(0, 0, simpleOpenGlControl.Width, simpleOpenGlControl.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();  //원근 행렬값 적용
            Glu.gluPerspective(80, (float)simpleOpenGlControl.Width / (float)simpleOpenGlControl.Height, 0.1, 200);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity(); //행렬값 적용

            Gl.glEnable(Gl.GL_DEPTH_TEST); //깊이 버퍼 테스트
        }

        private void Draw_Grid() //좌표 그리드를 그리는 함수
        {
            int gridNum = 200;
            Gl.glTranslatef(-100.0f, -15.0f, -200.0f);

            Gl.glColor3f(128, 128, 128);
            Gl.glPointSize(3.0f);


            Gl.glBegin(Gl.GL_LINES); //선을 그린다. 포인트 2개 필요
            for (float i = 0; i <= gridNum; i += 5.0f)
            {

                Gl.glVertex3f(i, 0, 0);
                Gl.glVertex3f(i, 0, gridNum);

                Gl.glVertex3f(0, 0, i);
                Gl.glVertex3f(gridNum, 0, i);

                Gl.glVertex3f(0, 0, i);
                Gl.glVertex3f(0, gridNum, i);

                Gl.glVertex3f(0, i, 0);
                Gl.glVertex3f(0, i, gridNum);

            };
            Gl.glEnd();

            Gl.glTranslatef(100.0f, 15.0f, 200.0f);
            simpleOpenGlControl.Invalidate();
        }



        private void Draw_Texture_Init() //텍스쳐를 그리기 위한 준비
        {

            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            Gl.glClearDepth(1.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            index = new int[1];
            Gl.glGenTextures(1, index);
        }

        public void Draw_Texture_Scene(float x, float y, float z)
        {

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); //화면과 깊이 버퍼를 지운다.
            Gl.glLoadIdentity(); //뷰 리셋
            Glu.gluLookAt(0.0, 0.0, 20.0, 0.0, 0.0, 0.0, 0.0, 5.0, 0.0); //카메라 시점 조정

            Draw_Grid(); //좌표 그리드 그리기

            Delay(50);
          
            Gl.glTranslatef(x, y, z);
           
             //Gl.glRotatef(xrot, 1, 0, 0); //그릴 화면 회전
             //Gl.glRotatef(yrot, 0, 1, 0);
             //Gl.glRotatef(zrot, 0, 0, 1);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]); //텍스쳐 바인딩
            
            Gl.glBegin(Gl.GL_QUADS); //사각형을 그린다.


            //원점 기준으로 맞추는 함수

            for (int i=0; i < noOfPoint-2; i++ )
                Set_Coordinate(i); //시작 점 인덱스


            for (int i = 0; i < 21; i+=2) //\포인트를 이용하여 벽을 그린다. ex) 0,1,2,3번 사각형, 다음은 2,3,4,5번 사각형
            {
                Gl.glTexCoord2f(0, 0); Gl.glVertex3f(coordSet[i].x, coordSet[i].y, coordSet[i].z); //사각형 좌표위치 선택
                Gl.glTexCoord2f(1, 0); Gl.glVertex3f(coordSet[i+2].x, coordSet[i+2].y, coordSet[i+2].z);
                Gl.glTexCoord2f(1, 1); Gl.glVertex3f(coordSet[i+3].x, coordSet[i+3].y, coordSet[i+3].z);
                Gl.glTexCoord2f(0, 1); Gl.glVertex3f(coordSet[i+1].x, coordSet[i+1].y, coordSet[i+1].z);



            }

            /*
            //모터카 충돌 평면
                Gl.glTexCoord2f(0, 0); Gl.glVertex3f(17, -2, 27);
                Gl.glTexCoord2f(1, 0); Gl.glVertex3f(30, -2, 27);
                Gl.glTexCoord2f(1, 1); Gl.glVertex3f(30, 2, 27);
                Gl.glTexCoord2f(0, 1); Gl.glVertex3f(17, 2, 27);
            // Bottom Face
            
                Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, -1, -1);
                Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, -1, -1);
                Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
                Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
            // Right face
                Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, -1);
                Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
                Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, 1);
                Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
            // Left Face
                Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, -1);
                Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
                Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, 1);
                Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
            */

                Gl.glEnd(); //사각형 그리기를 끝낸다.

                Gl.glColor3f(255, 0, 0); //색깔을 바꾼다.
                Gl.glTranslatef(20.0f, -20.0f, 5.0f); //그릴 위치 변경
                Glut.glutWireSphere(2, 50, 10); //와이어 구를 그린다.(원점 역할)
                Gl.glTranslatef(-20.0f, 20.0f, -5.0f); //그릴 위치 변경
                
               // xrot += 0.3f;
               // yrot += 0.2f;
               // zrot += 0.4f;

                Gl.glTranslatef(-x, -y, -z);// 화면 뷰 다시 전환


                simpleOpenGlControl.Invalidate(); //openGL화면 컨트롤 비활성화
        }

        private void Draw_Sphere(float x, float y, float z) //원점 구를 그리는 함수
        {
            Gl.glTranslatef(x, y, z);

            Gl.glColor3f(0.0f, 1.0f, 0.0f);

            Glut.glutWireSphere(10.0f, 50, 10);

            Gl.glTranslatef(-x, -y, -z);
        }


        //파일 Control 함수들

        private static Bitmap LoadBMP(string fileName) //bmp 파일 로드
        {
            if (fileName == null || fileName == string.Empty)
            {
                return null;
            }

            string fileName1 = string.Format("Data{0}{1}", Path.DirectorySeparatorChar, fileName);
            string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}", "..", Path.DirectorySeparatorChar, fileName);

            if (!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2))
            {
                return null;
            }

            if(File.Exists(fileName)){
                return new Bitmap(fileName);
            }
            else if(File.Exists(fileName1)){
                return new Bitmap(fileName1);
            }
            else if(File.Exists(fileName2)){
                return new Bitmap(fileName2);
            }

            return null;
        }

        private static bool LoadGLTextures() //벽 그림 텍스쳐를 불러온다.
        {
            bool status = false;
            Bitmap[] textureImage = new Bitmap[1];

            textureImage[0] = LoadBMP("wall.bmp"); //로드할 그림 파일

            if (textureImage[0] != null)
            {
                status = true;

                Gl.glGenTextures(1, texture); //텍스쳐를 만든다.

                for(int loop = 0; loop<textureImage.Length; loop++){ //텍스쳐가 여러개일 경우 여러개를 바인딩한다.
                textureImage[loop].RotateFlip(RotateFlipType.RotateNoneFlipY); //flip이란?
                Rectangle rectangle = new Rectangle(0, 0, textureImage[loop].Width, textureImage[loop].Height);

                BitmapData bitmapData = textureImage[loop].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[loop]); //텍스쳐를 2D로 바인딩한다.
               
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[loop].Width, textureImage[loop].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);



                   if (textureImage[loop] != null)
                  {
                      textureImage[loop].UnlockBits(bitmapData);
                      textureImage[loop].Dispose();
                  }
               }
            }


            return status;
        }


        //알고리즘 함수들

        private bool IsOutOfSpace(float x, float y, float z) //충돌검사 함수. 파라미터 x,y,z는 3차원 공간상의 충돌박스 직선끝의 한점을 의미한다.
        {
            float A, B, C, D, x1, y1, z1, x2, y2, z2, x3, y3, z3;
            bool result;


            // 첫번쨰 점
            x1 = p_array[0].x;
            y1 = p_array[0].y;
            z1 = p_array[0].z;
            // 두번쨰 점
            x2 = p_array[1].x;
            y2 = p_array[1].y;
            z2 = p_array[1].z;
            // 세번쨰 점
            x3 = p_array[2].x;
            y3 = p_array[2].y;
            z3 = p_array[2].z;



            A = y1 * (z2 - z3) + y2 * (z3 - z1) + y3 * (z1 - z2);
            B = z1 * (x2 - x3) + z2 * (x3 - x1) + z3 * (x1 - x2);
            C = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2);

            D = -(x1 * (y2 * z3 - y3 * z2) + x2 * (y3 * z1 - y1 * z3) + x3 * (y1 * z2 - y2 * z1));
            //3차원상 한 평면과 한점과의 관계를 나타내는 방정식. 

            float a = A * x + B * y + C * z + D;
            result = A * x + B * y + C * z + D < 0;
            //양수면 평면의 위쪽에, 음수면 평면의 아래쪽에 점이 존재한다. 따라서,
            //비교 값이 양수면 충돌, 음수면 안전 

            if (result)
                return false;
            else
                return true;

        }



        //Control Event 함수들
 
        private void move_Bt_Click(object sender, EventArgs e) //모터카를 네비게이션된 방향으로 이동시킨다.
        {
            serialPort1.Write("m");
        }

        private void stop_Bt_Click(object sender, EventArgs e) //모터카의 이동을 중지시킨다.
        {
            serialPort1.Write("s");
        }
        

        private void cancel_Bt_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Application.Exit();
            this.Close();
        }

        private void Initialize_coordinates()
        {
            x = new int[20];
            y = new int[20];
            z = new int[20];
        }

        private void Move_Command() //가속도 센서값으로 모터카의 이동을 결정
        {
            if((xAccelmeter < 650 && xAccelmeter > 350) && (yAccelmeter < 700 && yAccelmeter > 400)){
                totalDistance_box.Text = "정지";
                return;
            }

            if (xAccelmeter > 650)
                totalDistance_box.Text = "좌회전";
            else if (xAccelmeter < 370)
                totalDistance_box.Text = "우회전";
            else if (yAccelmeter > 700)
                totalDistance_box.Text = "후진";
            else
                totalDistance_box.Text = "직진";
        }

       
        //시리얼 포트 통신 준비
        private void SerialPortInitialize()
        {
            //사용 가능한 포트 콤보박스에 추가
            cmbPort.BeginUpdate();
            foreach (string comport in SerialPort.GetPortNames())
            {
                cmbPort.Items.Add(comport);
            }
            cmbPort.EndUpdate();

            serialPort1.Open(); // 포트 open
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            //이벤트 헨들러 추가  

            

        }

        private void convertTo3Dcoordinate()
        {
            double radius = 1.0;
            for (int k = 0; k < 24; k++)
            {
                coordSet[k].no = k;
                coordSet[k].x = (int)Math.Cos(radius);

                if (k % 2 == 0)
                    coordSet[k].y = -8;
                else
                    coordSet[k].y = 8;

                coordSet[k].z = (int)Math.Sin(radius);

                radius += 7.5;
            }
        }

        //시리얼 포트로 읽은 내용을 출력
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int i = 2;
       
            String xAccel, yAccel;

            xAccel = serialPort1.ReadLine();

            listBox1.Items.Add(xAccel);
            distance_count++;

            if (distance_count == 30)
                listBox1.Items.Clear();

           
/*
            if (mode == 0)
            {
                try
                {


                    xAccel = serialPort1.ReadLine();
                    yAccel = serialPort1.ReadLine();

                    if(xAccel.Equals("END"))
                    {
                       mode = 1;
                  
                    }

                    xAccel_box.Text = xAccel;
                    i = 3;
                    yAccel_box.Text = yAccel;
                    i = 4;

                    yAccelmeter = Convert.ToInt32(yAccel);
                    i = 0;

                    xAccelmeter = Convert.ToInt32(xAccel);
                    i = 1;

                    
                }
                catch (Exception except)
                {
                    String str = except.Message;
                    str = str + i;
                    MessageBox.Show(str);
                }

                Move_Command();
            }
            
            if (mode == 1) //거리 수신
            {
                String str;

                while(distance_count <24)
                {
                    str = serialPort1.ReadLine();
                    distance[distance_count++] = Convert.ToInt32(str);

                }

                mode = 1;

                convertTo3Dcoordinate();

                for (int g = 0; g < 24; g++)
                {
                    String str1 = "(" + coordSet[g].x + "," + coordSet[g].y + "," + coordSet[g].z + ")";
                    listBox1.Items.Add(str1);
                }

                Draw_Texture_Scene(-20.0f, -7.0f, -20.0f);

            }
            */

            /*
            if (serialPort1.IsOpen)
            {

                coordSet[cnt_point].no = serialPort1.ReadChar(); //좌표 번호를 읽는다.
                
                string str = serialPort1.ReadLine(); //new line 문자가 나올때까지 읽는다.
                
                string[] str_array;

                str_array = str.Split(new char [] {','}); // 콤마를 기준으로 숫자 String을 추출 후 String 배열에 각각 저장

                try
                {
                    coordSet[cnt_point].x = Convert.ToInt32(str_array[0]);  //문자를 숫자로 변환
                    coordSet[cnt_point].y = Convert.ToInt32(str_array[1]);
                    coordSet[cnt_point].z = Convert.ToInt32(str_array[2]);
                    
                    
                }
                catch (Exception except)
                {
                    MessageBox.Show(except.Message);
                }
                listBox1.Items.Add("좌표 "+ coordSet[cnt_point].no + " 수신");
               
            }

            cnt_point++;

            
            //좌표가 20개 모이면 한꺼번에 출력
            if (cnt_point == 14)
            {
                MessageBox.Show("14개의 좌표가 모두 수신되었습니다.");
               // cnt_point = 0;
                string str;
                listBox2.Items.Clear();
                for (int i = 0; i < 14; i++)
                {
                    str = "["+coordSet[i].no+"]"+"(" + coordSet[i].x + ", " + coordSet[i].y + ", " + coordSet[i].z + ")";
                    listBox2.Items.Add(str);
                }
            }


            if (cnt_point == 14)
            {
                serialPort1.Close();
            }
                */

        }

        private float p_a, p_b, p_c;
        private int[] p_x = new int[20], p_y = new int[20], p_z = new int[20];
        private int cnt_point;
        private float a1 = 20.0f, b1 = 20, c1 = 0;


        private void set_Bt_Click(object sender, EventArgs e)
        {
            b1 = b1 - 0.1f;

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); //화면과 깊이 버퍼를 지운다.
            Gl.glLoadIdentity(); //뷰 리셋
            Glu.gluLookAt(0.0, 0.0, a1--, 0.0, 0.0, 0.0, b1, 15.0, 0.0); //카메라 시점 조정

            Draw_Grid(); //좌표 그리드 그리기

            Delay(50);

            Gl.glTranslatef(-20.0f, -7.0f, -20.0f);

            //Gl.glRotatef(xrot, 1, 0, 0); //그릴 화면 회전
            //Gl.glRotatef(yrot, 0, 1, 0);
            //Gl.glRotatef(zrot, 0, 0, 1);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]); //텍스쳐 바인딩

            Gl.glBegin(Gl.GL_QUADS); //사각형을 그린다.


            //원점 기준으로 맞추는 함수

            for (int i = 0; i < noOfPoint - 2; i++)
                Set_Coordinate(i); //시작 점 인덱스


            for (int i = 0; i < 11; i += 2) //\포인트를 이용하여 벽을 그린다. ex) 0,1,2,3번 사각형, 다음은 2,3,4,5번 사각형
            {
                Gl.glTexCoord2f(0, 0); Gl.glVertex3f(coordSet[i].x, coordSet[i].y, coordSet[i].z); //사각형 좌표위치 선택
                Gl.glTexCoord2f(1, 0); Gl.glVertex3f(coordSet[i + 2].x, coordSet[i + 2].y, coordSet[i + 2].z);
                Gl.glTexCoord2f(1, 1); Gl.glVertex3f(coordSet[i + 3].x, coordSet[i + 3].y, coordSet[i + 3].z);
                Gl.glTexCoord2f(0, 1); Gl.glVertex3f(coordSet[i + 1].x, coordSet[i + 1].y, coordSet[i + 1].z);



            }


            Gl.glEnd(); //사각형 그리기를 끝낸다.

            Gl.glColor3f(255, 0, 0); //색깔을 바꾼다.
            Gl.glTranslatef(20.0f, -20.0f, 5.0f); //그릴 위치 변경
            Glut.glutWireSphere(2, 50, 10); //와이어 구를 그린다.(원점 역할)
            Gl.glTranslatef(-20.0f, 20.0f, -5.0f); //그릴 위치 변경

            Gl.glTranslatef(-20.0f, -7.0f, -20.0f);// 화면 뷰 다시 전환


            simpleOpenGlControl.Invalidate(); //openGL화면 컨트롤 비활성화

        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Write("l");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Write("r");
        }

    }


}