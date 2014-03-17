#AssociationRules

Plans: Implement Apriori Algorithm and its optimizations with C#

##Project **"Apriori"**:
Entry is ***Program.cs***  
Methodes are defined in ***Apriori.cs***
###Example1
*with minSupport = 3:*  
**input1.txt**  
*Each line represents a transaction, items within the transaction are divided by backslashes*  
>1\2\3\4  
1\2\4  
1\2  
2\3\4  
2\3  
3\4  
2\4  

**output1.txt**  
*Items between curly brackets forms an item set,  the number after the colon is the support*  
>{1 } : 3  
{2 } : 6  
{3 } : 4  
{4 } : 5  

>{1 2 } : 3  
{2 3 } : 3  
{2 4 } : 4  
{3 4 } : 3  

###Example2 
*with minSupport = 2:*  
**input2.txt**
>A\C\D  
B\C\E  
A\B\C\E  
B\E  

**output2.txt**
>{A } : 2  
{C } : 3  
{B } : 3  
{E } : 3  

>{A C } : 2  
{C B } : 2  
{C E } : 2  
{B E } : 3  

>{C B E } : 2  

###Known issues
Duplicate items in one transaction will lead to unexpected results. This will be fixed at next update.
