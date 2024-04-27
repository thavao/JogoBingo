/*
**JogoBingo**

Vamos desenvolver um jogo?

Simples, um jogo de bingo.

As regras do bingo são as seguintes:

- As cartelas possuem 25 números escritos em ordem aleatória.
- Os números sorteados vão de 1 a 99.
- Se algum jogador completar uma linha a pontuação para todos passa a valer somente a coluna de qualquer cartela e vice-versa.
- A partir daí, só vale a pontuação de cartela cheia.
- Os sorteios devem acontecer até algum jogador completar a cartela (BINGO!).
- São 3 possibilidades de pontos:
- O primeiro jogador a completar uma linha recebe 1 ponto.
- O primeiro jogador a completar uma coluna recebe 1 ponto.
- Ao completar a cartela, o jogador recebe 5 pontos.

Você vai precisar controlar o sorteio, onde não podem 
acontecer números repetidos, e também controlar as cartelas,
onde cada cartela deve ter marcado os números sorteados para verificação
 do preenchimento da linha / coluna / cartela para contabilizar os 
pontos.

Ao final do jogo, deverá ser mostrado quem foram os jogadores vencedores e a pontuação de cada um.

Recursos opcionais:

- Cada jogador pode ter mais de uma cartela.
- O jogo deve ser capaz de ser jogado por mais de 2 jogadores, onde o
usuário informa no inicio do programa a quantidade de jogadores que ele
deseja.
*/
//problema atual: criar mais de uma cartela

bool bingo = false;

int minimoSorteado = 1, maximoSorteado = 100;

int qtdLinhas = 5, qtdColunas = 6, qtdJogadores = 2, qtdCartelasPorJogador = 2;

int ColunaDadosNaCartela = 5, linhaJogadorNaCartela = 0, LinhaNumerosMarcados = 1;

bool pontoHorizontal = false, pontoVertical = false, tabelaCheia = false;

//jogadoresNumerosColunas[jogador][coluna, cartela]
int[,] JogadoresNumerosColunas = new int[qtdColunas - 1, qtdCartelasPorJogador * qtdJogadores];
int[,] JogadoresNumerosLinhas = new int[qtdLinhas, qtdCartelasPorJogador * qtdJogadores];

bool[] jogadorPontoHorizontal = new bool[qtdJogadores];
bool[] jogadorPontoVertical = new bool[qtdJogadores];
int[] pontosJogador = new int[qtdJogadores];

string[] jogadores = new string[qtdJogadores];


int[][,] cartela = new int[qtdCartelasPorJogador * qtdJogadores][,];

int[,] CartelasDoJogador = new int[qtdJogadores, qtdCartelasPorJogador + 1];

//int[] numerosMarcadosCartela = new int[25];

int[] numerosSorteados;
int[] numerosApresentados = new int[99];
int contadorNumerosApresentados = 0;


#region Funcoes
void ImprimirCartela(int[,] cartelaRecebida)
{
	bool sorteado = false;
	for (int linhaAtual = 0; linhaAtual < qtdLinhas; linhaAtual++)
	{
		Console.WriteLine();
        Console.Write("\t  ");
        for (int colunaAtual = 0; colunaAtual < qtdColunas - 1; colunaAtual++)
		{
			for (int i = 0; i < contadorNumerosApresentados; i++)
			{
				sorteado = false;


				if (numerosApresentados[i] == cartelaRecebida[linhaAtual, colunaAtual])
				{
					sorteado = true;
					break;
				}
			}
			if (sorteado)
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write(cartelaRecebida[linhaAtual, colunaAtual].ToString("00") + " ");
				Console.ResetColor();
			}
			else
				Console.Write(cartelaRecebida[linhaAtual, colunaAtual].ToString("00") + " ");
		}
	}
}

int[] GerarConjuntoAleatorioSemRepeticoes(int tamanho)
{
	int[] conjunto = new int[tamanho];

	int aux = new Random().Next(minimoSorteado, maximoSorteado);
	conjunto[0] = aux;

	for (int i = 1; i < tamanho; i++)
	{
		aux = new Random().Next(minimoSorteado, maximoSorteado);
		for (int j = 0; j < i; j++)
		{
			if (aux == conjunto[j])
			{
				i--;
				break;
			}
			else
			{
				conjunto[i] = aux;
			}
		}
	}
	return conjunto;

}

int[,] SortearCartela()
{
	int[] numerosDaCartela = GerarConjuntoAleatorioSemRepeticoes(25);

	int[,] Novatabela = new int[qtdLinhas, qtdColunas];
	int indiceVetor = 0;

	for (int linhaAtual = 0; linhaAtual < qtdLinhas; linhaAtual++)
	{
		for (int colunaAtual = 0; colunaAtual < qtdColunas - 1; colunaAtual++)
		{
			Novatabela[linhaAtual, colunaAtual] = numerosDaCartela[indiceVetor];
			indiceVetor++;
		}
	}
	return Novatabela;
}

int[,] AtualizarTabela(int numero, int[,] tabela, int jogador)
{

	for (int linhaAtual = 0; linhaAtual < qtdLinhas; linhaAtual++)
	{
		for (int colunaAtual = 0; colunaAtual < qtdColunas - 1; colunaAtual++)
		{
			if (tabela[linhaAtual, colunaAtual] == numero)
			{

				tabela[LinhaNumerosMarcados, ColunaDadosNaCartela]++;
				JogadoresNumerosColunas[jogador, colunaAtual]++;
				return tabela;
			}

		}
	}

	return tabela;
}

void AtualizarTabelas(int jogador, int numero)
{
	for (int c = 1; c <= qtdCartelasPorJogador; c++)
	{
		int indiceCartela = CartelasDoJogador[jogador, c];
		for (int linhaAtual = 0; linhaAtual < qtdLinhas; linhaAtual++)
		{
			for (int colunaAtual = 0; colunaAtual < qtdColunas - 1; colunaAtual++)
			{
				if (cartela[indiceCartela][linhaAtual, colunaAtual] == numero)
				{

					cartela[indiceCartela][LinhaNumerosMarcados, ColunaDadosNaCartela]++;
					JogadoresNumerosColunas[colunaAtual, indiceCartela]++;
					JogadoresNumerosLinhas[linhaAtual, indiceCartela]++;

					if (!pontoVertical && JogadoresNumerosColunas[colunaAtual, indiceCartela] >= 5)
					{
						pontoVertical = true;
						jogadorPontoVertical[jogador] = true;
						Console.WriteLine($"Jogador {jogadores[jogador]} marcou uma coluna!");
					}

				}

			}
			if (!pontoHorizontal && JogadoresNumerosLinhas[linhaAtual, indiceCartela] >= 5)
			{
				pontoHorizontal = true;
				jogadorPontoHorizontal[jogador] = true;
				Console.WriteLine($"Jogador {jogadores[jogador]} marcou uma linha!");

			}
		}
	}
}

void ImprimirCartelasJogador(int jogador)
{

	for (int colunaAtual = 1; colunaAtual <= qtdCartelasPorJogador; colunaAtual++)
	{
		Console.WriteLine();
		Console.WriteLine();
		Console.WriteLine($"    cartela {colunaAtual} do jogador: {jogadores[jogador]}");
		ImprimirCartela(cartela[CartelasDoJogador[jogador, colunaAtual]]);
	}


}

void ImprimirQtdNumerosMarcados(int jogador)
{
	for (int i = 1; i <= qtdCartelasPorJogador; i++)
	{
		int indiceCartela = CartelasDoJogador[jogador, i];
		Console.WriteLine($"   Marcados na cartela {i}: {cartela[indiceCartela][LinhaNumerosMarcados, ColunaDadosNaCartela]}");
	}
}

void ContarPontos()
{
	for (int i = 0; i < qtdJogadores; i++)
	{
		if (jogadorPontoHorizontal[i])
		{
			pontosJogador[i]++;
		}
		if (jogadorPontoVertical[i])
		{
			pontosJogador[i]++;
		}
	}
}

bool VerificarBingo(int jogador)
{
	int qtdMarcadosNaCartela = 0;
	for (int colunaAtual = 1; colunaAtual <= qtdCartelasPorJogador; colunaAtual++)
	{
		qtdMarcadosNaCartela = cartela[CartelasDoJogador[jogador, colunaAtual]][LinhaNumerosMarcados, ColunaDadosNaCartela];
		if (qtdMarcadosNaCartela == 25)
		{
			return true;
		}
	}
	return false;
}

void FinalizarJogo(int vencedor)
{
	pontosJogador[vencedor] += 5;
	Console.WriteLine("---BINGO!---");
	Console.WriteLine("O vencedor foi: " + jogadores[vencedor]);
	ContarPontos();
	for (int i = 0; i < qtdJogadores; i++)
	{
		Console.WriteLine($"Jogador {jogadores[i]} fez: {pontosJogador[i]} pontos");
	}

	Console.ReadLine();
}

#endregion

#region Mocando Dados
cartela[0] = SortearCartela();
cartela[1] = SortearCartela();
cartela[0][linhaJogadorNaCartela, ColunaDadosNaCartela] = 0;
cartela[1][linhaJogadorNaCartela, ColunaDadosNaCartela] = 0;
jogadores[0] = "José";
CartelasDoJogador[0, 1] = 0;
CartelasDoJogador[0, 2] = 1;

cartela[2] = SortearCartela();
cartela[3] = SortearCartela();
cartela[2][linhaJogadorNaCartela, ColunaDadosNaCartela] = 0;
cartela[3][linhaJogadorNaCartela, ColunaDadosNaCartela] = 3;
jogadores[1] = "Julio";
CartelasDoJogador[1, 1] = 2;
CartelasDoJogador[1, 2] = 3;
#endregion


numerosSorteados = GerarConjuntoAleatorioSemRepeticoes(99);
Console.WriteLine();


for (int i = 0; i < numerosSorteados.Length; i++)
{
	if (!bingo)
	{
		numerosApresentados[i] = numerosSorteados[i];
		contadorNumerosApresentados++;

		for (int jogador = 0; jogador < qtdJogadores; jogador++)
		{
			Console.WriteLine();
			Console.WriteLine("{-----------------------------------}");
			Console.WriteLine("\tJogador: " + jogadores[jogador]);
			ImprimirCartelasJogador(jogador);
			Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("   Numero Sorteado: " + numerosSorteados[i]);

			AtualizarTabelas(jogador, numerosSorteados[i]);

			ImprimirQtdNumerosMarcados(jogador);
			Console.WriteLine("{-----------------------------------}");
			if (VerificarBingo(jogador))
			{
				bingo = true;

				FinalizarJogo(jogador);
				break;
			}
			Console.ReadLine();

		}
	}
	else
		break;
	Console.Clear();
}

