# Adafruit-8x8-Controller
Tested on a Raspberry Pi 2 running Mono (Mono JIT compiler version 3.2.8 (Debian 3.2.8+dfsg-4+rpi1)) on Raspian (Linux raspberrypi 3.18.11-v7+ #781 SMP PREEMPT Tue Apr 21 18:07:59 BST 2015 armv7l GNU/Linux), with a [yellow-green Adafruit 8x8 Controller](http://www.adafruit.com/products/1051) connected. 

Should probably work on all Adafruit 8x8 matrices, but hasn't been tested.

# How to wire the LED matrix
The connectors on the matrix are (from left to right) +, -, D, and C, and should be wired against the Raspberry Pi in the following manner:
* + to 3.3 VDC
* - to GND
* D to SDA (data) (Physical pin 3)
* C to SCL (clock) (Physical pin 5)

Please refer to [this guide](http://pi.gadgetoid.com/pinout) for more information on pinout.
