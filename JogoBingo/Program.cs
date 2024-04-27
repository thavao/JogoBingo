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

bool bingo = false;

//constantes para numero minimo e numero maximo que podem estar no sorteio
const int minimoSorteado = 1, maximoSorteado = 100;

//constantes para tamanho das cartelas
const int qtdLinhas = 5, qtdColunas = 5 + 1;

//Constantes para referenciar dados da cartela na cartela
const int ColunaDadosNaCartela = 5, linhaJogadorNaCartela = 0, LinhaNumerosMarcados = 1;

//Variaveis para controle de quantidades
int qtdJogadores, qtdCartelasPorJogador;
int totalCartelas;

//variaveis para controlar pontos;
bool pontoHorizontal = false, pontoVertical = false;
int[,] JogadoresNumerosColunas;
int[,] JogadoresNumerosLinhas;

bool[] jogadorPontoHorizontal;
bool[] jogadorPontoVertical;
int[] pontosJogador;

//Variaveis para controlar jogadores e suas cartelas
const int ColunaDoJogador = 0; //indice da coluna que guarda os indices dos jogadores na cartelasDoJogador
string[] jogadores;
int[][,] cartela;
int[,] cartelasDoJogador;

//Variaveis para sorteio
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

void ChecarCartelas(int jogador, int numero)
{
	for (int c = 1; c <= qtdCartelasPorJogador; c++)
	{
		int indiceCartela = cartelasDoJogador[jogador, c];
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
		Console.WriteLine($"    Cartela {colunaAtual} do jogador: {jogadores[jogador]}");
		ImprimirCartela(cartela[cartelasDoJogador[jogador, colunaAtual]]);
	}


}

void ImprimirQtdNumerosMarcados(int jogador)
{
	for (int i = 1; i <= qtdCartelasPorJogador; i++)
	{
		int indiceCartela = cartelasDoJogador[jogador, i];
		Console.WriteLine($"    Marcados na cartela {i}: {cartela[indiceCartela][LinhaNumerosMarcados, ColunaDadosNaCartela]}");
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
		qtdMarcadosNaCartela = cartela[cartelasDoJogador[jogador, colunaAtual]][LinhaNumerosMarcados, ColunaDadosNaCartela];
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
    Console.WriteLine($"Foram: {contadorNumerosApresentados} rodadas!");
    Console.WriteLine("O vencedor foi: " + jogadores[vencedor]);
	ContarPontos();
	for (int i = 0; i < qtdJogadores; i++)
	{
		Console.WriteLine($"Jogador {jogadores[i]} fez: {pontosJogador[i]} pontos");
	}

	Console.ReadLine();
}
void CriarCartelas()
{
	totalCartelas = qtdCartelasPorJogador * qtdJogadores;
	cartela = new int[totalCartelas][,];
	JogadoresNumerosColunas = new int[qtdColunas - 1, totalCartelas];
	JogadoresNumerosLinhas = new int[qtdLinhas, totalCartelas];
	jogadorPontoHorizontal = new bool[qtdJogadores];
	jogadorPontoVertical = new bool[qtdJogadores];
	pontosJogador = new int[qtdJogadores];

	for (int i = 0; i < totalCartelas; i++)
	{
		cartela[i] = SortearCartela();
	}
}
void DefinirJogadores()
{
	Console.Write("Digite a quantidade de jogadores: ");
	qtdJogadores = int.Parse(Console.ReadLine());
	Console.WriteLine();
	Console.Write("Digite a quantidade de cartelas por jogador: ");
	qtdCartelasPorJogador = int.Parse(Console.ReadLine());

	CriarCartelas();

	Console.WriteLine();

	jogadores = new string[qtdJogadores];

	for (int i = 0; i < qtdJogadores; i++)
	{
		Console.Write($"Digite o nome do {i + 1}: ");
		jogadores[i] = Console.ReadLine();

	}
}

void DistribuirCartelas()
{
	int indiceCartela = 0;
	cartelasDoJogador = new int[qtdJogadores, qtdCartelasPorJogador + 1];
	for (int jogador = 0; jogador < qtdJogadores; jogador++)
	{
		cartelasDoJogador[jogador, ColunaDoJogador] = jogador;
		for (int colunaAtual = 1; colunaAtual <= qtdCartelasPorJogador; colunaAtual++)
		{
			cartelasDoJogador[jogador, colunaAtual] = indiceCartela;
			cartela[indiceCartela][linhaJogadorNaCartela, ColunaDadosNaCartela] = jogador;
			indiceCartela++;
		}
	}
}

#endregion







DefinirJogadores();
DistribuirCartelas();

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
			Console.WriteLine("\t  Jogador: " + jogadores[jogador]);
			ImprimirCartelasJogador(jogador);
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("    Numero Sorteado: " + numerosSorteados[i]);

			ChecarCartelas(jogador, numerosSorteados[i]);

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

