﻿RUN UP THE STAIRS

You are climbing a staircase. The staircase consists of some number of flights of stairs separated by landings. A
flight is a continuous series of steps from one landing to another. You are a reasonably tall athletic person, so you
can climb a certain number of steps in one stride. However, after each flight, there is a landing which you cannot
skip because you need to turn around for the next flight (which continues in a different direction).
You will be given the number of steps in each flight in a Integer[] flights. Element 0 of flights represents the
number of steps in the first flight, element 1 is the number of steps in the second flight, etc. You will also be given
an Integer StairsPerStride, which is how many continuous steps you climb in each stride. If it takes two strides to
turn around at a landing, return the number of strides to get to the top of the staircase. You do not need to turn at
the top of the staircase.
The staircase has between 1 and 50 flights of stairs, inclusive. Each flight of stairs has between 5 and 30 steps,
inclusive. StepsPerStride is between 2 and 5, inclusive.
Examples:
Input: {15}, StepsPerStride: 2, Returns: 8
A simple staircase with 15 steps. In 7 strides, you&#39;ve climbed 14 steps. However, you still have one step left, so
you must use an additional stride to get to the top.
Input: {15, 15}, StepsPerStride: 2, Returns: 18
This time, there are two flights with a landing in between. 8 strides to get to the first landing, 2 strides to turn
around, and 8 more strides to get to the top makes 8+2+8=18 strides.
Input: {5,11,9,13,8,30,14}, StepsPerStride: 3, Returns: 44