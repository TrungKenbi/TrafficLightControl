#include "Ticker.h"

byte trafficIndex = 0;
unsigned char trafficLight[3][3] = {{ 0, 255, 0 }, { 255, 255, 0 }, { 255, 0, 0 }};

void _timerCallback();
Ticker timer(_timerCallback, 0); 

void setup() {
  pinMode(11, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(13, OUTPUT);
  Serial.begin(9600);
  delay(2000);
  
  timer.start();
}

void loop() {
  timer.update();
  if (timer.counter() == 1)
    timer.interval(5000);
}


void color (unsigned char red, unsigned char green, unsigned char blue) {
  analogWrite(11, red);
  analogWrite(12, green);
  analogWrite(13, blue);
}

void serialEvent() {
  while (Serial.available()) {
    char inChar = (char)Serial.read();
    switch (inChar) {
      case '0': // Đèn xanh
        trafficIndex = 0;
        color(0, 255, 0);
        break;

      case '1': // Đèn vàng
        trafficIndex = 1;
        color(255, 255, 0);
        break;

      case '2': // Đèn đỏ
        trafficIndex = 2;
        color(255, 0, 0);
        break;

      case '3':
        color(0, 0, 0);
        break;
    }
    Serial.print(trafficIndex);
  }
}

void _timerCallback() {
  color(trafficLight[trafficIndex][0], trafficLight[trafficIndex][1], trafficLight[trafficIndex][2]);
  Serial.print(trafficIndex);
  trafficIndex++;
  if (trafficIndex > 2)
    trafficIndex = 0;
}
