// Stepping Motor Control Application
// Target : M128
// Crystal: 16Mhz
//
// Made by New Technology Company(N.T.C) in Korea.
// DAEWOO RYU   
// Email : davidryu@newtc.co.kr
// 82-2-704-4774(TEL), 82-2-704-4733(FAX)
// http://www.NewTC.co.kr
//


/*
 마이크로 스텝드라이버 구동 소스
 포트 연결
 Enable		: PORTB 7번핀
 Mode1		: PORTB 5번핀
 Mode0		: PORTB 4번핀
 R_DIR		: PORTB 3번핀
 R_CLK		: PORTB 2번핀
 L_DIR		: PORTB 1번핀
 L_CLK		: PORTB 0번핀
*/


//#include <iom128v.h>
//#include <macros.h>
//#include <stdio.h>

#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/stdlib.h>

#define MODE_MOVE	10
#define MODE_SENSING 11
#define MODE_TRANS 12

#define MOTOR_PORT		PORTC       // 스테핑 모터 연결 포트
#define MOTOR_PORT_DDR	DDRC        // 스테핑 모터 연결 포트의 DDR REG

#define MOTOR_ENABLE	(MOTOR_PORT = MOTOR_PORT|0x80)
#define MOTOR_DISABLE	(MOTOR_PORT = MOTOR_PORT&0x7f)
#define MOTOR_STEP_M0	(MOTOR_PORT = (MOTOR_PORT&0xcf)|0x00)
#define MOTOR_STEP_M1	(MOTOR_PORT = (MOTOR_PORT&0xcf)|0x10)
#define MOTOR_STEP_M2	(MOTOR_PORT = (MOTOR_PORT&0xcf)|0x20)
#define MOTOR_STEP_M3	(MOTOR_PORT = (MOTOR_PORT&0xcf)|0x30)
#define MOTOR_LEFT_CLK	(MOTOR_PORT = MOTOR_PORT^0x01)
#define MOTOR_RIGHT_CLK	(MOTOR_PORT = MOTOR_PORT^0x04)
#define MOTOR_LEFT_CW	(MOTOR_PORT = MOTOR_PORT&0xfd)
#define MOTOR_LEFT_CCW	(MOTOR_PORT = MOTOR_PORT|0x02)
#define MOTOR_RIGHT_CW	(MOTOR_PORT = MOTOR_PORT&0xf7)
#define MOTOR_RIGHT_CCW	(MOTOR_PORT = MOTOR_PORT|0x08)


#define UART0_BUF_SIZE 128
#define LEFT_MOT_ON (PORTD = PORTD | 0x20)
#define LEFT_MOT_R_ON (PORTD = (PORTD & 0xdf) | 0x10)
#define LEFT_MOT_OFF (PORTD = PORTD & 0xcf)

#define RIGHT_MOT_ON (PORTD = PORTD | 0x40)
#define RIGHT_MOT_R_ON (PORTD = (PORTD & 0xbf) | 0x80)
#define RIGHT_MOT_OFF (PORTD = PORTD & 0x3f) 


#define MOT_STOP (PORTD = PORTD & 0x0f)

#define CR_VALUE		0x0d

typedef unsigned char BYTE;


#define INTERVAL 3




volatile unsigned char sensing_count = 0;
volatile unsigned char kkk = 0;

volatile unsigned char RXD;

volatile unsigned char pulse_count1 = 0;
volatile unsigned char pulse_count2 = 0;

volatile unsigned char distance_count = 0;
volatile unsigned char pulse_count = 0;

volatile unsigned char togle = 0;
volatile unsigned char togle1 = 0;
volatile unsigned char light = 0;
volatile unsigned char togle2 = 0;
volatile unsigned char togle3 = 0;

volatile unsigned char count = 0;
volatile unsigned char count2 = 0;
volatile unsigned char distance[24];
volatile unsigned char begin = 0;
volatile unsigned char end = 0;
volatile unsigned char serial_count = 0;
float zero_count = 0;
float test_count = 0;

unsigned char rx0_buf[UART0_BUF_SIZE];
volatile unsigned char sensing_check = 0;

unsigned int p_rx0_wr, p_rx0_rd;

unsigned char cr0_check_flag;


volatile unsigned char command;

volatile unsigned char mode; //1번 모드 : 초음파 센싱
                             //2번 모드 : 모터카 이동
							 //3번 모드 : 조음파 센싱 좌표 전송

volatile unsigned char safe_mode = 0;

SIGNAL(SIG_USART_RECV)
{
	while((UCSRA & 0x80) == 0x00);

	rx0_buf[p_rx0_wr++] = UDR;
	if(rx0_buf[p_rx0_wr-1] == CR_VALUE) cr0_check_flag++;

	if(p_rx0_wr > UART0_BUF_SIZE-1) p_rx0_wr = 0;
}



void port_init(void)
{



	PORTB = 0x00;
	DDRB  = 0xFF;
	PORTC = 0x00;
	DDRC  = 0xff;
	PORTD = 0x00;
	DDRD = 0xFA;


	DDRA = 0xFF;
	PORTA = 0x00;

	ACSR = 0x80;
	SREG = 0x80;
}


//UART0 initialize
// desired baud rate: 9600
// actual: baud rate:9615 (0.2%)
// char size: 8 bit
// parity: Disabled

void Delay_us(BYTE time_us)
{
   BYTE i;

   for(i=0;i<time_us;i++){
      asm volatile(" PUSH R0");
      asm volatile(" POP  R0");
      asm volatile(" PUSH R0");
      asm volatile(" POP  R0");
      asm volatile(" PUSH R0");
      asm volatile(" POP  R0");
   }
}

void Delay_ms(unsigned int time_ms)
{
   unsigned int i;

   for(i=0;i<time_ms;i++){
      Delay_us(250);
      Delay_us(250);
      Delay_us(250);
	  Delay_us(250);
   }
}

unsigned char RX0_char(void)
{
	while((UCSRA & 0x80) == 0x00);
		return UDR;
}


unsigned char GETCHAR_UART0(void)
{
	unsigned char ch;
	ch = rx0_buf[p_rx0_rd++];
	if(p_rx0_rd > UART0_BUF_SIZE-1) p_rx0_rd = 0;

	return ch;
	
}



void TX0_char(unsigned char data)
{
	///PORTG = ~PORTG;
	while((UCSRA & 0x20) == 0x00);
	UDR = data;

}

void TX0_string(char *string)
{
	while(*string != '\0')
	{
		TX0_char(*string);
		Delay_ms(10);
		string++;
	}
}

void Move_Motor_R()
{
		MOTOR_RIGHT_CW;
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
}

void Move_Motor_L()
{
		MOTOR_RIGHT_CCW;
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;
		Delay_ms(30);
		MOTOR_RIGHT_CLK;

}

void pulse1(void)
{
	PORTB = 0x01;
	Delay_us(10);
	PORTB = 0x00;
	Delay_ms(10);
}

void pulse2(void)
{
	PORTA = 0x01;
	Delay_us(10);
	PORTA = 0x00;
	Delay_ms(10);
}

SIGNAL(SIG_OVERFLOW0)
{

	count2++;

	if(count2 == 170){
				GICR = 0x40; //INT0 인터럽트 인에이블
  				MCUCR = 0x03; //INT0 라이징 에지
				MCUCSR = 0x00;
				SREG = 0x80;
				GIFR = 0x40; //INTF0 클리어
				pulse1();
				togle2 = 0;
			}
	if(count2 == 180){
				DDRD = 0xF2;
				GICR = 0x80; //INT1 인터럽트 인에이블
  				MCUCR = 0x0C; //INT1 라이징 에지
				MCUCSR = 0x00;
				SREG = 0x80;
				GIFR = 0x80; //INTF1 클리어
				pulse2();
				togle2 = 0;
				count2 = 160;
		}

	TCNT0 = 160;			
}

SIGNAL(SIG_OVERFLOW2)
{
	pulse_count++;

	TCNT2 = 145; //2tic에 1us 이므로 120us 가 1cm 일때 시작값

}


SIGNAL(SIG_INTERRUPT1)
{
//	char dist[4];
//	char str[4];

	if(togle2 == 0)
	{
	    pulse_count= 0;
	   	TIMSK = 0x41;
		TCNT2 = 145;
		TCCR2 = 0x02; //타이머 시작
		MCUCR = 0x08; //INT1 falling 에지
		togle2 = 1;

	}
	else
	{
		TIMSK = 0x01;
		TCCR2 = 0x00; //타이머 종료
		end = pulse_count; //pulse_count는 58us가 지날때 마다 1씩 증가.
		                   //초음파의 속도(v)로 58us(t)가 지나면 1cm(s) 이다.
		togle2 = 0;	
		
		MCUCR = 0x0C; //INT0 rising 에지
		//PORTA = end;
		distance[distance_count++] = end;

		//itoa(end, dist, 10);
		
		DDRD = 0xFA;
		GICR = 0x00;

		//2상 여자 방식이므로 2번 클럭이 들어가야 1스텝이 돈다.
		//한번에 2스텝씩 이동
		
		if(mode == MODE_SENSING)
			Move_Motor_R();
		PORTB = 0x01;
		//PORTB = 0x01;
		Delay_ms(1000);
	
		sensing_count++;
//		itoa(mode, str, 10);
//		TX0_string(str);
//		TX0_char(0x0d);TX0_char(0x0a);
		Delay_ms(30);
		if(sensing_count == 12 && mode == MODE_SENSING){
			sensing_count = 0;
			TCCR0 = 0x00; 
			TCCR2 = 0x00; 
			TIMSK = 0x00;
			mode = MODE_TRANS;

		}
	}
	
}


SIGNAL(SIG_INTERRUPT0)
{
	char dist[4];
	if(togle == 0)
	{
	    pulse_count= 0;
	   	TIMSK = 0x41;
		TCNT2 = 145;
		TCCR2 = 0x02; //타이머 시작
		
		MCUCR = 0x02; //INT0 falling 에지
		togle = 1;

	}
	else
	{
		TIMSK = 0x01;
		TCCR2 = 0x00; //타이머 종료
		end = pulse_count; //pulse_count는 58us가 지날때 마다 1씩 증가.
		                   //초음파의 속도(v)로 58us(t)가 지나면 1cm(s) 이다.
		togle = 0;	
		
		MCUCR = 0x03; //INT0 rising 에지
		//PORTA = end;
		distance[distance_count++] = end;
		
		PORTB = 0x02;

		itoa(end, dist, 10);
		
		//TX0_string("1 : ");
	//	TX0_string(dist);
	//	TX0_string("cm ");
		

	}
	
}




void uart_init(void)
{	

 UCSRC = 0x00 | 1 << UCSZ1 | 1 << UCSZ0;


 //BAUD RATE 설정을 위로 올리면 안된다. UCSRC 보다 먼저 설정되면 안된다.
 //UBRRL = 0x04; //set baud rate lo
 UBRRL = 0x67;
 UBRRH = 0x00; //set baud rate hi

 UCSRA = 0x00;
 UCSRB = 0x98;



}


//call this routine to initialize all peripherals
void init_devices(void)
{
	//stop errant interrupts until set up
	cli(); //disable all interrupts
//	XDIV  = 0x00; //xtal divider
//	XMCRA = 0x00; //external memory
	port_init();
	uart_init();



	p_rx0_wr = 0;
	p_rx0_rd = 0;


	for(int i=0; i<UART0_BUF_SIZE; i++) rx0_buf[i] = 0;

//	TIMSK = 0x41;
//	TCCR0 = 0x05;
//	TCNT0 = 160;

	sei(); //re-enable interrupts
	//all peripherals are now initialized
}

unsigned char CHECK_RX_BUF_UART0(void)
{
	unsigned char len;

	if(p_rx0_wr == p_rx0_rd)
	{
		len = 0;
	}
	else
	{
		if(p_rx0_wr - p_rx0_rd > 0) len = p_rx0_wr - p_rx0_rd;
		else
			len = UART0_BUF_SIZE + p_rx0_wr - p_rx0_rd;

	}

	return len;

}






/*
void BYPASS_MODE(void)
{

	if(CHECK_RX_BUF_UART0())
	{
		command = GETCHAR_UART0();
		PUTCHAR_UART1(command);
	}

	if(CHECK_RX_BUF_UART0())
	{
		PUTCHAR_UART0(GETCHAR_UART1());
	}
}
*/
	

/* main function-----------------------------------*/
int main(void)
{
//	volatile int count4 = 0;
	volatile int i = 0;
	char str[4];
	init_devices();
	char ok_state = 0;
	char post_command;

//	char cnt = 0;

	//insert your functional code here...
		
  	MOTOR_PORT_DDR = 0xff;

	MOTOR_ENABLE,
	MOTOR_STEP_M2;
    MOTOR_RIGHT_CCW;

while(ok_state == 0){

	if(CHECK_RX_BUF_UART0())
	{		
		command = GETCHAR_UART0();

		//자동 페어링
		if(command == 'K')
			ok_state=1;

		
		if(ok_state == 1){
			TX0_string("AT+BTSCAN");
			TX0_char(0x0d);TX0_char(0x0a);
			PORTB |= 0x04;
			ok_state=99;}

		i++;
	}
}

while(mode == 0)
{
	
	if(CHECK_RX_BUF_UART0())
	{		
		command = GETCHAR_UART0();


		if(command == 'm')
			mode = MODE_MOVE;
		else if(command == 's')
			mode = MODE_SENSING;
	}

}

	while(1){

if(mode == MODE_MOVE){

//PORTB |= 0x00;
/*	if(safe_mode == 1)
	{
		TIMSK = 0x41; //타이머 시작
		TCCR0 = 0x05;
		TCNT0 = 160;
		safe_mode = 0;
	}
*/
	//	PORTB = 0x02;
	if(command == '4'){
		MOT_STOP;
		}
	else if(command == '3'){
		MOT_STOP;
		//PORTB |= 0x02;

		RIGHT_MOT_R_ON;
		LEFT_MOT_R_ON;
		}
	else if(command == '2'){
		MOT_STOP;

		RIGHT_MOT_ON;
		LEFT_MOT_ON;
		//전진			
		}
	else if(command == '5'){
			//좌회전
		MOT_STOP;
		RIGHT_MOT_ON;
		LEFT_MOT_OFF;
		Delay_ms(150);
		MOT_STOP;
		}
	else if(command == '6'){
		//우회전
		MOT_STOP;
		LEFT_MOT_ON;
		RIGHT_MOT_OFF;
		Delay_ms(150);
		MOT_STOP;		
		}
	else
	{
		;
	}

	Delay_ms(10);
	if(CHECK_RX_BUF_UART0())
	{		
		command = GETCHAR_UART0();
		//TX0_char(command);

		i++;

		if(command == 's'){
			mode = MODE_SENSING;
			sensing_check = 0;
			sensing_count = 0;
			TIMSK = 0x00;
			}
		else if(command == 'l')
			Move_Motor_L();
		else if(command == 'r')
			Move_Motor_R();
	}

	Delay_ms(10);
	//PORTB = ~PORTB;

}

else if(mode == MODE_SENSING){

	if(sensing_check == 0)
	{
		TIMSK = 0x00;
		//PORTB = ~PORTB;
		for(i=0; i<6; i++)
			Move_Motor_L(); //맨 왼쪽으로 이동
		Delay_ms(10);
		


		Delay_ms(10);
		distance_count = 0;
		for(i=0; i<24; i++)
			distance[i] = 0;
		
		distance_count = 0;
		TIMSK = 0x41; //타이머 시작
		TCCR0 = 0x05;
		TCNT0 = 160;

		sensing_check = 1;

	}
}
else if(mode == MODE_TRANS)
{		


		for(i=0; i<24; i++)
		{PORTB = ~PORTB;
			itoa(distance[i++], str, 10); //거리값 character로 변환
			TX0_string(str);
			Delay_ms(30);

			TX0_char(' ');
			Delay_ms(30);

			itoa(distance[i], str, 10); //거리값 character로 변환
			TX0_string(str);
			Delay_ms(30);
			TX0_char(0x0d);TX0_char(0x0a);
			Delay_ms(30);
			
			
		}

		distance_count = 0;

		TX0_char(0x0d);TX0_char(0x0a);
		for(i=0; i<6; i++)
			Move_Motor_L(); //가운데로 이동
		

		mode = 10;
		safe_mode = 1;

		
			
}
else
{
		mode = 10;
		safe_mode = 1;
}


}
	          //모터 Half모드에서 한바퀴 96스텝
	
	return 0;
}
