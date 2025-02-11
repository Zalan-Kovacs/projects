#include <stdio.h>
#include <stdlib.h>

void mainMenu();
void userGuide();
void generateSpiralMatrix(int n, char start[7], char rotation[4], int ***matrix);
void saveMatrix(int n, char start[7], char rotation[4], int **matrix);
void loadMatrix(int *n, int ***matrix);
void freeMatrix(int n, int **matrix);

int main()
{
    int choice;
    int n = 0;
    char start[7] = "";
    char rotation[4] = "";
    int **currentMatrix = NULL;
    do
    {
        mainMenu();
        printf("\nYour Choice >> ");
        scanf("%d", &choice);

        if(choice == 0)
        {
            userGuide();
        }
        else if(choice == 1)
        {
            freeMatrix(n, currentMatrix);
            printf("\nAdja meg a matrix dimenziojat (1-20 kozott): ");
            scanf("%d", &n);

            printf("Indulasi irany kivalasztasa(balra, jobbra, fel vagy le): ");
            scanf("%s", start);

            printf("Forgasi irany kivalasztasa (cw: oramutato jarassal megegyezo, ccw: ellentetes): ");
            scanf("%s", rotation); 
            
            generateSpiralMatrix(n, start, rotation, &currentMatrix);
            
        }
        
        else if(choice == 2)
        {
            if(currentMatrix != NULL)
            {
                saveMatrix(n, start, rotation, currentMatrix);
            }
            else
            {
                printf("Nincs mentheto matrix, eloszor hozzon letre egyet!");
            }
        }
        else if(choice == 3)
        {
            freeMatrix(n, currentMatrix);
            loadMatrix(&n, &currentMatrix);
        }
        else if(choice == 4)
        {
            printf("\n\n");
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    printf(" %3d",currentMatrix[i][j]);
                }
                printf("\n");
            }
        printf("\n");
            
        }
        else if(choice == 5)
        {
            printf("Viszontlatasra!");
            freeMatrix(n, currentMatrix);
        }
        else
        {
            printf("\nErvenytelen valasztas, kerem valasszon masikat!\n");
        }
    } 
    while (choice != 5);
    
    return 0;
}

void mainMenu ()
{   
    printf("\n###################################");
    printf("\n|     Spiral Matrix Generator     |");
    printf("\n|#################################|");
    printf("\n|    0  -  User  Guide       -    |");
    printf("\n|    1  -  Generate Matrix   -    |");
    printf("\n|    2  -  Save Matrix       -    |");
    printf("\n|    3  -  Load Matrix       -    |");
    printf("\n|    4  -  Print  Matrix     -    |");
    printf("\n|    5  -  E X I T           -    |");
    printf("\n###################################");
}
void userGuide()
{
    printf("\nEz itt az utmutato!");
    printf("\nA program spiral matrixok letrehozasaval foglalkozik.");
    printf("\n\nMukodese:");
    printf("\nA(z) 0. billentyu lenyomasaval megnyithatja ezt a sugot.");
    printf("\nA(z) 1. billentyu lenyomasaval legeneralhatja a matrixot. Ehhez 3 erteket var a program: a matrix dimenziojat (1-20), az indulasi es a forgatasi iranyt.");
    printf("\nA(z) 2. billentyu lenyomasaval lementheti az aktualis matrixot.");
    printf("\nA(z) 3. billentyu lenyomasaval betolthet egy fajlbol egy matrixot.");
    printf("\nA(z) 4. billentyu lenyomasaval kiirathatja az aktualis matrixot.");
    printf("\nA(z) 5. billentyu lenyomasaval kilephet a programbol.\n");
    
}
void freeMatrix(int n, int **matrix)
{
    for(int i = 0; i < n; i++)
    {
        free(matrix[i]);       
    }
    free(matrix);
}
void saveMatrix(int n, char start[7], char rotation[4], int **matrix)
{
    char name[50];
    sprintf(name, "spiral%d%s%s.txt", n, start, rotation);

    FILE *file = fopen(name, "w");

    fprintf(file, "%d\n", n);
    for(int i = 0; i < n; i++)
    {
        for(int j = 0; j < n; j++)
        {
            fprintf(file, "%d ", matrix[i][j]);
        }
        fprintf(file, "\n");
    }
    fclose(file);
    printf("A fajl sikeresen mentve lett %s neven.\n", name);
}
void loadMatrix(int *n, int ***matrix)
{
    char name[50];

    printf("Beolvasando fajl neve: ");
    scanf("%s", name);

    FILE *file = fopen(name, "r");

    if(file == NULL)
    {
        printf("\nA fajl valoszinuleg nem letezik.\n");
    }

    fscanf(file, "%d", n);

    *matrix = (int **)malloc(*n * sizeof(int *));
    for(int i = 0; i < *n; i++)
    {
        (*matrix)[i] = (int *)malloc(*n * sizeof(int));
    }

    for(int i = 0; i < *n; i++)
    {
        for(int j = 0; j < *n; j++)
        {
            fscanf(file, "%d", &(*matrix)[i][j]);
        }
    }
    fclose(file);
}
void generateSpiralMatrix(int n, char start[7], char rotation[4], int ***matrix)
{
     *matrix = (int **)malloc(n * sizeof(int *));
    for(int i = 0; i < n; i++)
    {
        (*matrix)[i] = (int *)malloc(n * sizeof(int));
    }

    int row, col;
    int value = 1;
    
    if(start[0] == 'j' && start[1] == 'o' && start[2] == 'b' && start[3] == 'b' && rotation[0] == 'c' && rotation[1] == 'w')
    {
        if((n % 2) != 0)
        {   
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {
                
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
            }
        }
        else
        {
            row = (n/2-1);
            col = (n/2-1);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {
                
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row - (n/2 - 1) + i][col + n/2] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row + n/2][col + n/2 - i] = value++;
            }
        }
    }
    else if(start[0] == 'j' && start[1] == 'o' && start[2] == 'b' && start[3] == 'b' && rotation[0] == 'c' && rotation[1] == 'c' && rotation[2] == 'w')
    {
        if((n % 2) != 0)
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
            }
        }
        else
        {
            row = (n/2);
            col = (n/2-1);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row + (n/2 - 1) - i][col + n/2] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row - n/2][col + n/2 - i] = value++;
            }
        }
    }
    else if(start[0] == 'b' && start[1] == 'a' && start[2] == 'l' && rotation[0] == 'c' && rotation[1] == 'w')
    {
        if((n % 2) != 0)
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
            }
        }
        else
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row + (n/2 - 1) - i][col - n/2] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row - n/2][col - n/2 + i] = value++;
            }
        }
    }
    else if(start[0] == 'b' && start[1] == 'a' && start[2] == 'l' && rotation[0] == 'c' && rotation[1] == 'c' && rotation[2] == 'w')
    {
        if((n % 2) != 0)
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
            }
        }
        else
        {
            row = (n/2 - 1);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row - (n/2 - 1) + i][col - n/2] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row + n/2][col - n/2 + i] = value++;
            } 
        }
    }
    else if(start[0] == 'l' && start[1] == 'e' && rotation[0] == 'c' && rotation[1] == 'w')
    {
        if((n % 2) != 0)
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {   
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
            }
        }
        else
        {
            row = (n/2 - 1);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {   
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row + n/2][col + n/2 -1 - i] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row + n/2 - i][col - n/2] = value++;
            }
            
        }
    }
    else if(start[0] == 'l' && start[1] == 'e' && rotation[0] == 'c' && rotation[1] == 'c' && rotation[2] == 'w')
    {
        if((n % 2) != 0)
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {   
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
            }
        }
        else
        {
            row = (n/2 - 1);
            col = (n/2 - 1);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {   
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row + n/2][col - n/2 + 1 + i] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row + n/2 - i][col + n/2] = value++;
            }
            
        }
    } 
    else if(start[0] == 'f' && start[1] == 'e' && start[2] == 'l' && rotation[0] == 'c' && rotation[1] == 'w')
    {
        if((n % 2) != 0)
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
            }
            
        }
        else
        {
            row = (n/2);
            col = (n/2 - 1);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col + i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col - i] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row - n/2][col - n/2 + 1 + i] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row - n/2 + i][col + n/2] = value++;
            }
        }
    }
    else if(start[0] == 'f' && start[1] == 'e' && start[2] == 'l' && rotation[0] == 'c' && rotation[1] == 'c' && rotation[2] == 'w')
    {
        if((n % 2) != 0)
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= n/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
            }
        }
        else
        {
            row = (n/2);
            col = (n/2);
            (*matrix)[row][col] = value++;
            
            for(int i = 1; i <= (n-1)/2; i++)
            {
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i][col + i - j - 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row - i + j + 1][col - i] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i][col - i + j + 1] = value++;
                }
                for(int j = 0; j < 2 * i; j++)
                {
                    (*matrix)[row + i - j - 1][col + i] = value++;
                }
            }
            for(int i = 0; i < n; i++)
            {
                (*matrix)[row - n/2][col + n/2 - 1 - i] = value++;
            }
            for(int i = 1; i < n; i++)
            {
                (*matrix)[row - n/2 + i][col - n/2] = value++;
            }
        }
    }
    else
    {
        printf("Helytelen bemenet, probalja ujra!");
    }
}

