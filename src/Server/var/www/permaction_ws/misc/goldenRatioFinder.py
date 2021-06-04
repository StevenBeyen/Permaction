#! /usr/bin/env python
# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from math import sqrt

def ratioFinder(ratio, deviation, size, step):
	lowLimit = ratio - deviation
	highLimit = ratio + deviation
	values = []
	a = step
	while (a < sqrt(size)):
		b = round(size/a, 1)
		ratio = b/a
		if (int(a) * int(b) == size) and (lowLimit <= ratio <= highLimit):
			values.append((int(a),int(b)))
		a += step
	return values



phi = (1+sqrt(5))/2
dev = phi/50
step = 1.0

res = {}

for i in range (0, 20):
	values = ratioFinder(phi, dev*5, i, step)
	if (values):
		res[i] = values

for i in range (0, 5000):
	values = ratioFinder(phi, dev, i, step)
	if (values):
		res[i] = values

print(res)


#for i in range (500, 5000, 10):
#	values = ratioFinder(phi, dev, i, step)
#	if (values):
#		print(i,values)

