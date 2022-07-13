using System;
using System.Threading;

namespace APD___Multithreading
{
	class Program
	{
		static void Main(string[] args)
		{
			//pozitiile pe care se afla diferitele tipuri de campuri de pe tabla de joc
			int[] communityChest = { 2, 17, 33 };
			int[] chance = { 7, 22, 36 };
			int[] train = { 5, 15, 25, 35 };
			//dupa fiecare pozitie a campurilor de tip taxa se afla si costul lor
			int[] tax = { 4, 200, 12, 150, 28, 150, 38, 100 };

			//pozitiile si pretul proprietatilor de pe tabla de joc; o proprietate se afla acolo unde pozitia este diferita de 0, iar valoare de pe acea pozitie este costul proprietatii
			int[] propertyPrices = { 0, 60, 0, 60, 0, 0, 100, 0, 100, 120, 0, 140, 0, 140, 160, 0, 180, 0, 180, 200, 0, 220, 0, 220, 240, 0, 260, 260, 0, 280, 0, 300, 300, 0, 320, 0, 0, 350, 0, 400 };

			//vector in care se pastreaza ce proprietati detine fiecare jucator
			int[] playerProperty = new int[40];
			for (int i = 0; i < 40; i++)
			{
				playerProperty[i] = 0;
			}

			//pozitia, creditul si statusul de inchis al fiecarui jucator
			int positionP1 = 0, positionP2 = 0;
			int creditP1 = 1500, creditP2 = 1500;
			int jailedP1 = 0, jailedP2 = 0;

			//array-ul de threaduri thread1 se ocupa de primul jucator, iar array-ul de threaduri thread2 se ocupa de al doilea jucator
            Thread[] thread1 = new Thread[100];
            Thread[] thread2 = new Thread[100];

			//jocul are 100 de runde; o runda = fiecare jucator arunca cate o data cu zarul; jocul se termina cand un jucator ramane fara credit
			//daca dupa 100 de runde un jucator nu a ramas fara credit, atunci jucatorul cu cel mai mult credit castiga
			int count = 0;
            while(creditP1 != 0 && creditP2 != 0)
            {
				thread1[count] = new Thread(() => player1Turn(ref creditP1, ref positionP1, ref creditP2, propertyPrices, playerProperty, train, tax, ref jailedP1, chance, communityChest));
				thread1[count].Start();
				thread1[count].Join();

				thread2[count] = new Thread(() => player2Turn(ref creditP2, ref positionP2, ref creditP1, propertyPrices, playerProperty, train, tax, ref jailedP2, chance, communityChest));
				thread2[count].Start();
				thread2[count].Join();

				count++;
                if (count == 99)
                {
					Console.WriteLine("Toate rundele s-au epuizat! Castigatorul este jucatorul cu cel mai mult credit.");
                    if (creditP1 > creditP2)
                    {
						Console.WriteLine("Player1 a castigat!");
					}else if (creditP2 > creditP1)
                    {
						Console.WriteLine("Player2 a castigat!");
					}else if (creditP1 == creditP2)
                    {
						Console.WriteLine("Egalitate!");
					}
				}
			}
        }

		//randul primului jucator
        static void player1Turn(ref int credit, ref int position, ref int creditP2, int[] propertyPrices, int[] playerProperty, int[] train, int[] tax, ref int jailed, int[] chance, int[] communityChest)
		{
			//daca nu este inchis, jucatorul arunca cu zarul
			//daca trece de start atunci jucatorul primeste 200 credit
			//se verifica pe ce camp se afla si se ia actiunea necesara
			if(jailed == 0)
            {
				string playerThrow;
				int diceThrow;

				Console.WriteLine("Player 1 [Credit: " + credit + "][Position: " + position + "]:");
				Console.WriteLine("------------------------------------");
				Console.WriteLine("Apasati R pentru a arunca cu zarul!");
				playerThrow = Console.ReadLine();
				while (playerThrow != "R")
				{
					Console.WriteLine("Invaid. Reintroduceti.");
					playerThrow = Console.ReadLine();
				}
				diceThrow = roll();
				Console.WriteLine("Rezultat: " + diceThrow + ".");
				position = position + diceThrow;
				if (position >= 40)
				{
					credit = credit + 200;
					Console.WriteLine("Ati trecut de start! +200 credit.");
					position = position - 40;
				}
				Console.WriteLine("Va aflati pe pozitia " + position + ".");
				checkProperty(1, position, ref credit, ref creditP2, propertyPrices, playerProperty);
				checkTrain(ref position, ref credit, train);
				checkTax(position, ref credit, tax);
				checkCorners(ref position, ref jailed);
				checkCommunity(ref position, ref credit, communityChest);
				checkChance(ref position, ref credit, ref creditP2, chance);
				Console.WriteLine();
            }
            else
            {
				jailed--;
            }
		}

		//randului jucatorului secund
		static void player2Turn(ref int credit, ref int position, ref int creditP1, int[] propertyPrices, int[] playerProperty, int[] train, int[] tax, ref int jailed, int[] chance, int[] communityChest)
        {
			//daca nu este inchis, jucatorul arunca cu zarul
			//daca trece de start atunci jucatorul primeste 200 credit
			//se verifica pe ce camp se afla si se ia actiunea necesara
			if (jailed == 0)
            {
				string playerThrow;
				int diceThrow;

				Console.WriteLine("Player 2 [Credit: " + credit + "][Position: " + position + "]:");
				Console.WriteLine("------------------------------------");
				Console.WriteLine("Apasati R pentru a arunca cu zarul!");
				playerThrow = Console.ReadLine();
				while (playerThrow != "R")
				{
					Console.WriteLine("Invaid. Reintroduceti.");
					playerThrow = Console.ReadLine();
				}
				diceThrow = roll();
				Console.WriteLine("Rezultat: " + diceThrow + ".");
				position = position + diceThrow;
				if (position >= 40)
				{
					credit = credit + 200;
					Console.WriteLine("Ati trecut de start! +200 credit.");
					position = position - 40;
				}
				Console.WriteLine("Va aflati pe pozitia " + position + ".");
				checkProperty(2, position, ref credit, ref creditP1, propertyPrices, playerProperty);
				checkTrain(ref position, ref credit, train);
				checkTax(position, ref credit, tax);
				checkCorners(ref position, ref jailed);
				checkCommunity(ref position, ref credit, communityChest);
				checkChance(ref position, ref credit, ref creditP1, chance);
				Console.WriteLine();
            }
            else
			{
				jailed--;
            }
		}

		//aruncarea de zaruri
		static int roll()
		{
			//se genereaza 2 numere la intamplare intre 1 si 6 si este returnata suma lor
			Random dice = new Random();
			int dice1 = dice.Next(1, 7);
			int dice2 = dice.Next(1, 7);
			return dice1 + dice2;
		}

		//veriticare daca jucatorul este pe un camp de tip property
		static void checkProperty(int player, int position, ref int credit, ref int creditEnemy, int[] propertyPrices, int[] playerProperty)
		{
			if (propertyPrices[position] != 0)
			{
				//daca proprietatea nu este detinuta de nimeni
				if (playerProperty[position] == 0)
				{
					Console.WriteLine("Puteti cumpara aceasta proprietate pentru " + propertyPrices[position] + "$. Y/N?");
					string answer;
					answer = Console.ReadLine();
					while (answer != "Y" && answer != "N")
					{
						Console.WriteLine("Invalid. Reintroduceti Y/N: ");
						answer = Console.ReadLine();
					}
					if (answer == "Y" && credit >= propertyPrices[position])
					{
						Console.WriteLine("Ati cumparat aceasta proprietate. -" + propertyPrices[position] + " credit");
						credit = credit - propertyPrices[position];
						playerProperty[position] = player;
					}
					else if (answer == "Y" && credit < propertyPrices[position])
					{
						Console.WriteLine("Nu aveti destul credit pentru a cumpara proprietatea.");
					}
					else if (answer == "N")
					{
						Console.WriteLine("Nu ati cumparat aceasta proprietate.");
					}
				}
				else if (playerProperty[position] == player)
				{
					Console.WriteLine("Detineti deja aceasta proprietate.");
				}
				else
				{
					Console.WriteLine("Aceasta proprietate apartine playerului adversar. Trebuie sa platiti " + propertyPrices[position] + "$. -" + propertyPrices[position] + " credit");
					credit = credit - propertyPrices[position];
					creditEnemy = creditEnemy + propertyPrices[position];
				}
			}
		}

		//veriticare daca jucatorul este pe un camp de tip train
		static void checkTrain(ref int position, ref int credit, int[] train)
        {
			int nextPosition;
			string answer;
			for(int i = 0; i < 4; i++)
            {
                if (position == train[i])
                {
					//nextPosition este gara la care jucatorul urmeaza sa fie trimis daca doreste sa calatoreasca
                    if (i == 3)
                    {
						nextPosition = 0;
                    }
                    else
                    {
						nextPosition = i + 1;
                    }
					Console.WriteLine("Va aflati la o gara. Puteti calatori la pozitia " + train[nextPosition] + " pentru 200$. Y/N?");
					answer = Console.ReadLine();
					while (answer != "Y" && answer != "N")
					{
						Console.WriteLine("Invalid. Reintroduceti Y/N: ");
						answer = Console.ReadLine();
					}

                    if (answer == "Y")
                    {
						Console.WriteLine("Ati folosit gara si va aflati la pozitia " + train[nextPosition] + ". -200 credit");
						position = train[nextPosition];
						credit = credit - 200;
                        if (i == 3)
                        {
							Console.WriteLine("Ati trecut de start! +200 credit.");
							credit = credit + 200;
						}
					}
					else if (answer == "N")
                    {
						Console.WriteLine("Nu ati folosit gara.");
					}
					break;
				}
            }
        }

		//veriticare daca jucatorul este pe un camp de tip tax
		static void checkTax(int position, ref int credit, int[] tax)
        {
			for(int i = 0; i < 7; i = i + 2)
            {
                if (position == tax[i])
                {
					Console.WriteLine("Trebuie sa platiti o taxa de " + tax[i + 1] + "$. -" + tax[i + 1] + " credit");
					credit = credit - tax[i + 1];
				}
            }
        }

		//veriticare daca jucatorul este pe un camp de tip corner
		static void checkCorners(ref int position, ref int jailed)
        {
			//pozitia 10 = jail
			//pozitia 20 = free parking
			//pozitia 30 = go to jail
			if (position == 10)
            {
				Console.WriteLine("Ati intrat la inchisoare. Playerul advers arunca cu zarul de doua ori.");
				jailed++;
            }else if (position == 20)
            {
				Console.WriteLine("Parcare gratis pentru aceasta runda.");
			}else if (position == 30)
            {
				Console.WriteLine("Sunteti trimis la inchisoare. Va reintoarceti la pozitia 10. Playerul advers arunca cu zarul de doua ori.");
				position = 10;
				jailed++;
            }
        }

		//veriticare daca jucatorul este pe un camp de tip chance
		static void checkChance(ref int position, ref int credit, ref int creditEnemy, int[] chance)
        {
			for(int i = 0; i < 3; i++)
            {
                if (position == chance[i])
                {
					//este generat un numar la intamplare intre 1 si 3 si se ia actiunea corespunzatoare numarului generat
					string drawCard;
					Console.WriteLine("Apasati D pentru a trage o carde de tip chance!");
					drawCard = Console.ReadLine();
					while (drawCard != "D")
					{
						Console.WriteLine("Invaid. Reintroduceti.");
						drawCard = Console.ReadLine();
					}

					Random card = new Random();
					int drawnCard = card.Next(1, 4);

					if (drawnCard == 1)
					{
						Console.WriteLine("Inaintati la GO!");
						Console.WriteLine("Ati trecut de start! +200 credit");
						credit = credit + 200;
						position = 0;
					}
					else if (drawnCard == 2)
					{
						Console.WriteLine("Mergeti inapoi cu 3 spatii.");
						position = position - 3;
					}
					else if (drawnCard == 3)
					{
						Console.WriteLine("Playerul advers trebuie sa va plateasca 150$. +150 credit");
						credit = credit + 150;
						creditEnemy = creditEnemy - 150;
					}
				}
            }
		}

		//veriticare daca jucatorul este pe un camp de tip community
		static void checkCommunity(ref int position, ref int credit, int[] communityChest)
        {
			for(int i = 0; i < 3; i++)
            {
                if (position == communityChest[i])
                {
					//este generat un numar la intamplare intre 1 si 3 si se ia actiunea corespunzatoare numarului generat
					string drawCard;
					Console.WriteLine("Apasati D pentru a trage o carde de tip community chest!");
					drawCard = Console.ReadLine();
					while (drawCard != "D")
					{
						Console.WriteLine("Invaid. Reintroduceti.");
						drawCard = Console.ReadLine();
					}

					Random card = new Random();
					int drawnCard = card.Next(1, 4);

					if (drawnCard == 1)
					{
						Console.WriteLine("Plateste factura la spital! -200 credit");
						credit = credit - 200;
					}
					else if (drawnCard == 2)
					{
						Console.WriteLine("Este ziua ta! +100 credit");
						credit = credit + 100;
					}
					else if (drawnCard == 3)
					{
						Console.WriteLine("Plateste factura la scoala! -50 credit");
						credit = credit - 50;
					}
				}
            }
        }
    }
}