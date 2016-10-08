#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/stdlib.h>

#define INTERVAL 3
#define UART0_BUF_SIZE 128
#define UART1_BUF_SIZE 128

#define CR_VALUE		0x0d
#define MODE_MOVE	10;
#define MODE_SENSING 11;
#define MODE_TRANS 12;


// UART Function //
int  Data, Data_1000, Data_100, Data_10, Data_1;

const char FND_TABLE[11] = {0x3F, 0x06, 0x5B, 0x4F, 0x66, 0x6D, 0x7D, 0x07                     
        // FND NUMBER table    0,    1,    2,    3,    4,    5,    6,    7
                          , 0x7F, 0x6F, 0x00};
        //                ,    8,    9  space
int toggle;

volatile char mode;

unsigned char rx0_buf[UART0_BUF_SIZE];
unsigned char rx1_buf[UART1_BUF_SIZE];
unsigned int p_rx1_wr, p_rx1_rd;
unsigned int p_rx0_wr, p_rx0_rd;

unsigned char cr0_check_flag;
unsigned char cr1_check_flag;

SIGNAL(SIG_UART0_RECV)
{
	while((UCSR0A & 0x80) == 0x00);

	rx0_buf[p_rx0_wr++] = UDR0;
	if(rx0_buf[p_rx0_wr-1] == CR_VALUE) cr0_check_flag++;
/*	if(cr0_check_flag > 0)
	{
		if(rx0_buf[p_rx0_wr-1] == LF_VALUE)lf_check_flag++;
	}*/
	if(p_rx0_wr > UART0_BUF_SIZE-1) p_rx0_wr = 0;
//	PORTA = ~PORTA;
}

SIGNAL(SIG_UART1_RECV)
{
	while((UCSR1A & 0x80) == 0x00);

	rx1_buf[p_rx1_wr++] = UDR1;
	if(rx1_buf[p_rx1_wr-1] == CR_VALUE) cr1_check_flag++;

	if(p_rx1_wr > UART1_BUF_SIZE-1) p_rx1_wr = 0;
}

void Delay_us(unsigned int count)
{

	for(int i=0; i<count; i++){
		asm("PUSH R0");
		asm("POP R0");
		asm("PUSH R0");
		asm("POP R0");
		asm("PUSH R0");
		asm("POP R0");
		}
}

void Delay_ms(unsigned int count)
{

	for(int i=0; i<count; i++)
	{
		Delay_us(250);
		Delay_us(250);
		Delay_us(250);
		Delay_us(250);
	}

}


unsigned char RX0_char_scan(void)
{
	if((UCSR0A & 0x80) == 0x00)
		return 0x00;
	else
		return UDR0;
}

unsigned char RX0_char(void)
{
	while((UCSR0A & 0x80) == 0x00);
		return UDR0;
}




void TX0_char(unsigned char data)
{
	
	while((UCSR0A & 0x20) == 0x00);
	UDR0 = data;

}

void TX0_string(char *string)
{
	while(*string != '\0')
	{
		PORTA = 0xFE;
		TX0_char(*string);
		string++;
		PORTA = 0xFF;
	}
}

unsigned char RX1_char_scan(void)
{
	if((UCSR1A & 0x80) == 0x00)
		return 0x00;
	else
		return UDR1;
}

unsigned char RX1_char(void)
{
	while((UCSR1A & 0x80) == 0x00);
		return UDR1;
}




void TX1_char(unsigned char data)
{

	while((UCSR1A & 0x20) == 0x00);
	UDR1 = data;

}

void TX1_string(char *string)
{
	while(*string != '\0')
	{	
		TX1_char(*string);
		string++;
	}
}


void Return()
{
	unsigned char ch1, ch2;
	

	for(int i=0; i<100 && !(ch1 = RX0_char_scan()); i++);
	if(ch1 != 0){
		TX0_char(ch1);
		TX1_char(ch1); //지그비로 ASCII 문자 전송
	}
	
	for(int i=0; i<100 && !(ch2 = RX1_char_scan()); i++);
	if(ch2 != 0)
		TX0_char(ch2); //PC로 ASCII 문자 전송


/*
	while((UCSR1A & 0x20) == 0x00);
	UDR1 = ch;	
*/

}

/*
void Listen()
{
	unsigned char ch;
	while(!(ch = RX1_char_scan()));
		

//		TX0_char(ch);

	while((UCSR1A & 0x20) == 0x00);
	UDR1 = ch;	

}
*/

// main

void init_uart()
{
	DDRA = 0xFF;
	DDRE = 0xFE;
	DDRG = 0xFF;
	DDRD = 0xFB;

	PORTG = 0x00;
	PORTA = 0x00;

	UBRR0H = 0;
	UBRR0L = 0x67;
	UCSR0A = 0x00;
	UCSR0B = 0x98;
	UCSR0C = 0x00 | 1 << UCSZ01 | 1 << UCSZ00;

	UBRR1H = 0;
	UBRR1L = 0x67;
	UCSR1A = 0x00;
	UCSR1B = 0x98;
	UCSR1C = 0x00 | 1 << UCSZ11 | 1 << UCSZ10;

	p_rx0_wr = 0;
	p_rx0_rd = 0;
	p_rx1_wr = 0;
	p_rx1_rd = 0;

	for(int i=0; i<UART0_BUF_SIZE; i++) rx0_buf[i] = 0;
	for(int i=0; i<UART1_BUF_SIZE; i++) rx1_buf[i] = 0;

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

unsigned char CHECK_RX_BUF_UART1(void)
{
	unsigned char len;

	if(p_rx1_wr == p_rx1_rd)
	{
		len = 0;
	}
	else
	{
		if(p_rx1_wr - p_rx1_rd > 0) len = p_rx1_wr - p_rx1_rd;
		else
			len = UART1_BUF_SIZE + p_rx1_wr - p_rx1_rd;

	}
	return len;

}

unsigned char GETCHAR_UART0(void)
{
	unsigned char ch;
	ch = rx0_buf[p_rx0_rd++];
	if(p_rx0_rd > UART0_BUF_SIZE-1) p_rx0_rd = 0;

	return ch;
	
}

unsigned char GET_UART0(void)
{
	unsigned char ch;
	ch = rx0_buf[p_rx0_rd];
	return ch;
	
}

void PUTCHAR_UART0(unsigned char ch)
{
	while((UCSR0A & 0x20) == 0x00);
	UDR0 = ch;	

}

unsigned char GETCHAR_UART1(void)
{
	unsigned char ch;
	ch = rx1_buf[p_rx1_rd++];
	if(p_rx1_rd > UART1_BUF_SIZE-1) p_rx1_rd = 0;

	return ch;
	
}


void PUTCHAR_UART1(unsigned char ch)
{
	while((UCSR1A & 0x20) == 0x00);
	UDR1 = ch;	

}

void BYPASS_MODE(void)
{
	if(CHECK_RX_BUF_UART0())
	{
		PUTCHAR_UART1(GETCHAR_UART0());
	}
	if(CHECK_RX_BUF_UART1())
	{
		PUTCHAR_UART0(GETCHAR_UART1());
	}
}

void RETURN_CH(void)
{
	if(CHECK_RX_BUF_UART0())
	{
		PUTCHAR_UART0(GETCHAR_UART0());
	}
}

int main(void)
{
	unsigned char RXD;
	unsigned char current_state = 3;
	unsigned char i=0;
	unsigned int x_val, y_val;
	char str1[6], str2[5];
	char ok_state = 0;
	char mode = 0;
	char tmp1, tmp2;
	init_uart();
	unsigned char transport_str;
	ADCSRA = 0xA5;	//ADC Enable 및 Freerunning Mode 설정, 분주비 /32
	ADMUX = 0x41;   //AREF 사용, ADC1사용
	Delay_us(150);
	ADCSRA = 0xE5; //ADC Start Conversion
	Delay_ms(1);

	RXD = UDR0;
	RXD = UDR1; 


	sei();

	while(ok_state == 0){
		if(CHECK_RX_BUF_UART0())
		{
			transport_str = GETCHAR_UART0();
			PUTCHAR_UART1(transport_str);

			
		}
		if(CHECK_RX_BUF_UART1())
		{
			transport_str = GETCHAR_UART1();
			PUTCHAR_UART0(transport_str);
			if(transport_str == 'D');
				ok_state = 1;
			
		}

	}

	while(1)
	{

	if(mode == 10){		
		PORTG = 0x00;
		x_val = 0;
		y_val = 0;
	
		ADMUX = 0x41;
		for(i=0; i<16; i++)
		{
			x_val += ADCL + ADCH*256;
			Delay_ms(1);
		}

		x_val = (int)((x_val >> 4) * 1.5f);

		ADMUX = 0x42;
		for(i=0; i<16; i++)
		{
			y_val += ADCL + ADCH*256;
			Delay_ms(1);
		}

		y_val = (int)((y_val >> 4) * 1.5f);
		
		if(x_val < 200 || y_val < 200)
			continue;
		
		itoa(x_val, str1, 10);
		itoa(y_val, str2, 10);

		tmp1 = str1[0];
		for(i=0; i<4; i++){
			tmp2 = str1[i+1];
			str1[i+1] = tmp1;
			tmp1 = tmp2;
		}

		tmp1 = str2[0];
		for(i=0; i<4; i++){
			tmp2 = str2[i+1];
			str2[i+1] = tmp1;
			tmp1 = tmp2;
		}

		str1[0] = '*'; //x 가속도값 구분 문자
		str2[0] = '>'; //y 가속도값 구분 문자

		if((x_val < 650 && x_val >350) && (y_val < 700 && y_val > 400)){
			if(current_state != 4)
			{
			current_state = 4;
			TX1_char('4'); //정지명령 4번
			TX1_char(0x0D);
			
			Delay_ms(100);
			TX1_char('4'); //정지명령 4번
			TX1_char(0x0D);
			Delay_ms(10);

			TX0_string("[4 : stop]");
			TX0_char(0x0D);
			TX0_char(0x0A);			
			Delay_ms(100);
			//PORTG = ~PORTG;
			}
		}
		else if(x_val > 650){
			if(current_state != 6)
			{
			current_state = 6;
			TX1_char('6'); //좌회전명령 6번
			TX1_char(0x0D);
			TX0_string("[6 : turn right]");
			TX0_char(0x0D);
			TX0_char(0x0A);
			}
		}
		else if(x_val < 370){
			if(current_state != 5)
			{
			current_state = 5;
			TX1_char('5'); //우회전명령 5번
			TX1_char(0x0D);
			TX0_string("[5 : turn left]");
			TX0_char(0x0D);
			TX0_char(0x0A);
			}
		}	
		else if(y_val > 700){
			if(current_state != 2)
			{
			current_state = 2;
			TX1_char('2'); //직진명령 2번
			TX1_char(0x0D);	
			TX0_string("[2 : go straight]");
			TX0_char(0x0D);
			TX0_char(0x0A);
			}		
		}
		else if(y_val <350){
			if(current_state != 3)
			{
			current_state = 3;
			TX1_char('3'); //후진명령 3번
			TX1_char(0x0D);
			TX0_string("[3 : go back]");
			TX0_char(0x0D);
			TX0_char(0x0A);
			}		
		}

		Delay_ms(30);


		TX0_string(str1); //x축값 전송
		TX0_char(0x0D);
		TX0_char(0x0A);
		Delay_ms(30);
		TX0_string(str2); //y축값 전송
		TX0_char(0x0D); 
		TX0_char(0x0A);


		
	
	if(CHECK_RX_BUF_UART0())
	{
		transport_str = GETCHAR_UART0();
		PUTCHAR_UART1(transport_str);

		if(transport_str == '2')
		{
			TX0_string("[2 : go straight]");
			TX0_char(0x0D);
			TX0_char(0x0A);
		}
		else if(transport_str == '3')
		{
			TX0_string("[3 : go back]");
			TX0_char(0x0D);
			TX0_char(0x0A);
		}
		else if(transport_str == '4')
		{
			TX0_string("[4 : stop]");
			TX0_char(0x0D);
			TX0_char(0x0A);
		}
		else if(transport_str == '5')
		{
			TX0_string("[5 : turn left]");
			TX0_char(0x0D);
			TX0_char(0x0A);
		}
		else if(transport_str == '6')
		{
			TX0_string("[6 : turn right]");
			TX0_char(0x0D);
			TX0_char(0x0A);
		}

		if(transport_str == 'm')
			mode = 10; //MODE_MOVE
		else if(transport_str == 's'){
			mode = 12; //MODE_SENSING
			TX0_string("trans");
			TX0_char(0x0d);TX0_char(0x0a);
			}
		else
			mode = 9;

	}
	if(CHECK_RX_BUF_UART1())
	{
		transport_str = GETCHAR_UART1();
		PUTCHAR_UART0(transport_str);	
	}

	Delay_ms(10);

}
else{

	if(CHECK_RX_BUF_UART0())
	{
		transport_str = GETCHAR_UART0();
		PUTCHAR_UART1(transport_str);

		if(transport_str == 'm')
			mode = 10; //MODE_MOVE
		else if(transport_str == 's'){
			mode = 12; //MODE_SENSING
			TX0_string("trans");
			TX0_char(0x0d);TX0_char(0x0a);
		}
	}
	if(CHECK_RX_BUF_UART1())
	{
		transport_str = GETCHAR_UART1();
		PUTCHAR_UART0(transport_str);
	}


	Delay_ms(10);
}

}

}
