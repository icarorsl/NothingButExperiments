[Test]
[TestCaseSource(&quot;Add_Source&quot;)]
public AddResult Add_UsingARecursiveAlgorithm_ValuesAreAdded(byte[] f, byte[] s)
{
// Arrange
// Act
var result = AddRecursive(f, s);
// Assert
return new AddResult(f, s, result);
}
E.G.
Input : { 1, 1, 1 }, { 1, 1, 1 }
Result: {2,2,2}
Input : { 1, 1, 255 }, {0, 0, 1 }
Result: {1,2,0}
Conditions:
 You can assume inputs f &amp; s are never null, and are always of the same length.
 The algorithm should be non-destructive to the inputs.
 The algorithm should be able to handle large input lengths, of a couple of thousand values, but the input
will never be large enough to cause a stack overflow.
Solve with a recursive method.

FOUND THESE REFERENCES
http://ebanshi.cc/questions/5005227/asp-net-addition-of-two-byte-arrays

https://stackoverflow.com/questions/31844188/interview-test-adding-using-a-recursive-algorithm-c-sharp