using System;
using Xunit;

namespace Spanify.Net.UnitTests
{
    public class CommaSeparatedSpanCollectionTests
    {
        [InlineData("val0")]
        [InlineData("val0,val1")]
        [InlineData("val0,val1,val2")]
        [Theory]
        public void Enumeration_GetEnumerator_return_items_in_correct_order(string input)
        {
            var expectedValues = input.Split(',');

            var actual = input.AsSpan().Split(',');
            var actualEnumerator = actual.GetEnumerator();

            foreach (ReadOnlySpan<char> expectedValue in expectedValues)
            {
                Assert.True(actualEnumerator.MoveNext());
                Assert.True(expectedValue.SequenceEqual(actualEnumerator.Current));
            }
            Assert.False(actualEnumerator.MoveNext());
        }
        
        [InlineData("val0")]
        [InlineData("val0,val1")]
        [InlineData("val0,val1,val2")]
        [Theory]
        public void Enumeration_Foreach_can_use_the_enumerator(string input)
        {
            var expectedValues = input.Split(',');

            var actualValues = input.AsSpan().Split(',');

            foreach (var expected in expectedValues)
            {
                bool containValue = false;
                foreach (ReadOnlySpan<char> actual in actualValues)
                {
                    if (actual.SequenceEqual(expected.AsSpan()))
                    {
                        containValue = true;
                        break;
                    }
                }
                Assert.True(containValue);
            }
        }
        
        [InlineData("val0")]
        [InlineData("val0,val1")]
        [InlineData("val0,val1,val2")]
        [Theory]
        public void Get_return_correct_items_per_index(string input)
        {
            string[] expectedValues = input.Split(',');

            var actualValues = input.AsSpan().Split(',');

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.True(expectedValues[i].AsSpan().SequenceEqual(actualValues[i]));
            }
        }

        [InlineData("val0")]
        [InlineData("val0,val1")]
        [InlineData("val0,val1,val2")]
        [Theory]
        public void Count_returns_correct_amount_of_items(string data)
        {
            int expectedCount = data.Split(',').Length;

            int actualCount = data.AsSpan().Split(',').Count();
            
            Assert.Equal(expectedCount, actualCount);
        }
    }
}