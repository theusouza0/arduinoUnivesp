/*
* Projeto Integrador Univesp 
*
* EDUARDO LUIZ ANDRETTA
* JOAO VICTOR DE SANTANA SILVA
* JULIANO DOS SANTOS
* LUCAS CAMPOS DE SOUZA 
* MATHEUS CAMPOS DE SOUZA
*
*/
 
//Bibliotecas LiquidCrystal (LCD)
#include <LiquidCrystal.h>

//Definir pinos
#define SensorTemp		A1
#define SensorUmi		A0
#define Valvula			10
  
//Define os pinos que serão utilizados para ligação ao display
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);
 
void setup()
{
  //Define o número de colunas e linhas do LCD
  lcd.begin(16, 2);
}
 
void loop()
{
  int temperatura = 28 ;
  int umidade = 30 ;
  
  //lendo sensores
  temperatura = analogRead(SensorTemp); //Valor Raw
  umidade = analogRead(SensorUmi);//Valor Raw
  
  //temp ( 0 , 358)
  //umi (0 , 876)
  temperatura= map(temperatura,0,358,0,70);//escala
  umidade= map(umidade,0,876,0,100);
 
  //Limpa a tela
  lcd.clear();
  //Posiciona o cursor na coluna 0, linha 0;
  lcd.setCursor(0, 0);
  //Envia o texto entre aspas para o LCD
  lcd.print("Temper=");
  lcd.setCursor(8, 0);
  lcd.print(temperatura);
  lcd.setCursor(0, 1);
  lcd.print("Umidade=");
  lcd.setCursor(8, 1);
  lcd.print(umidade);
  delay(1000);
  
  if (temperatura > 35) {
    digitalWrite(Valvula, HIGH);
    lcd.setCursor(11,0);
    lcd.print("Agua");
    delay(3000);
    digitalWrite(Valvula,LOW);
    lcd.setCursor(11,0);
    lcd.print(" ");
  }
  
  if (umidade < 30) {
    digitalWrite(Valvula, HIGH);
    lcd.setCursor(11,0);
    lcd.print("Agua");
    delay(3000);
    digitalWrite(Valvula,LOW);
    lcd.setCursor(11,0);
    lcd.print(" ");
  }
}
