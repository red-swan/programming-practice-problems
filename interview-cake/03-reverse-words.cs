public static void ReverseWords(char[] message)
{
    // First we reverse all the characters in the entire message array
    ReverseCharacters(message, 0, message.Length - 1);

    // This gives us the right word order,
    // but with each word backward.
    // Now we'll make the words forward again
    // by reversing each word's characters

    // We hold the index of the *start* of the current word
    // as we look for the *end* of the current word
    int currentWordStartIndex = 0;
    for (int i = 0; i <= message.Length; i++)
    {
        // Found the end of the current word!
        if (i == message.Length || message[i] == ' ')
        {
            // If we haven't exhausted the array our
            // next word's start is one character ahead
            ReverseCharacters(message, currentWordStartIndex, i - 1);
            currentWordStartIndex = i + 1;
        }
    }
}

public static void ReverseCharacters(char[] message, int leftIndex, int rightIndex)
{
    // Walk towards the middle, from both sides
    while (leftIndex < rightIndex)
    {
        // Swap the left char and right char
        char temp = message[leftIndex];
        message[leftIndex] = message[rightIndex];
        message[rightIndex] = temp;
        leftIndex++;
        rightIndex--;
    }
}