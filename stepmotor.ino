void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
pinMode(8,OUTPUT);
pinMode(9,OUTPUT);
pinMode(10,OUTPUT);
pinMode(11,OUTPUT);

digitalWrite(8,LOW);
digitalWrite(9,LOW);
digitalWrite(10,LOW);
digitalWrite(11,LOW);
}

void loop() {
  char c =Serial.read();
  if (c=='r')
 {
  
 
digitalWrite(8,HIGH);
delay(10);
digitalWrite(8,LOW);

digitalWrite(9,HIGH);
delay(10);
digitalWrite(9,LOW);

digitalWrite(10,HIGH);
delay(10);
digitalWrite(10,LOW);

digitalWrite(11,HIGH);
delay(10);
digitalWrite(11,LOW);
}
else if (c=='l')
{
  digitalWrite(11,HIGH);
delay(10);
digitalWrite(11,LOW);

digitalWrite(10,HIGH);
delay(10);
digitalWrite(10,LOW);

digitalWrite(9,HIGH);
delay(10);
digitalWrite(9,LOW);

digitalWrite(8,HIGH);
delay(10);
digitalWrite(8,LOW);

}
}

