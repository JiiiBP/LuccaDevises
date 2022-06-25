# Lucca Devises

## Introduction

Ce dossier contient le test technique back LuccaDevises dont le but est de créer un programme permettant de convertir automatiquement des montants dans une devise souhaitée.

En voici la consigne :

PROBLÈME SUR LE LANGAGE C#

Gestion des devises

Vous souhaitez créer un petit programme permettant de convertir automatiquement des montants
dans une devise souhaitée.
Vous disposez des informations suivantes :
 Un montant dans une devise initiale
 Une devise cible
 Une liste incomplète de taux de change

En vous servant de la liste des taux de change, vous devez arriver à convertir le montant dans la
devise cible.

Par exemple, si vous voulez 550 Euros (EUR) en Yens (JPY), et que vous avez cette liste de taux de
change :
Devise de départ Devise d'arrivée Taux de change
AUD CHF 0.9661
JPY KWU 13.1151
EUR CHF 1.2053
AUD JPY 86.0305
EUR USD 1.2989
JPY INR 0.6571

Pour convertir les EUR en JPY, vous devez convertir le montant des dépenses en CHF, puis en AUD,
puis en JPY, en utilisant les taux suivants :
Devise de départ Devise d'arrivée Taux de change
EUR CHF 1.2053
AUD CHF 0.9661
AUD JPY 86.0305

Afin de faciliter le calcul, les taux de change sont arrondis à 4 décimales. Chaque étape intermédiaire
de calcul doit être arrondie à 4 décimales, et vous devez restituer le montant final arrondi sous la
forme d'un nombre entier positif.

Dans le cas de l'exemple le résultat est donc le suivant :
EUR --> CHF : 550 * 1.2053 = 662.9150
CHF --> AUD : 662.9150 * (1/0.9661) = 686.1833 (Attention, ici nous inversons le taux car le taux
fourni est AUD --> CHF et nous voulons CHF --> AUD. L'inversion doit également être arrondie à 4
décimales, donc 1/0.9661 = 1.0351
AUD --> JPY : 686.1833 * 86.0305 = 59033 (arrondi à l'entier)

Le résultat attendu est donc 59033.

Si plusieurs chemins de conversion vous permettent d'atteindre la devise cible, vous devez utiliser le
chemin le plus court.

Entrée du programme
Le programme doit pouvoir être exécuté avec la ligne de commande suivante :
LuccaDevises <chemin vers le fichier>

Ce fichier est structuré de la manière suivante :
La première ligne contient :
 La devise initiale D1 dans laquelle le montant est affiché, sous la forme d'un code de 3
caractères
 Le montant M dans cette devise initiale, sous la forme d'un nombre entier positif > 0
 La devise cible D2 vers laquelle il veut convertir le montant, sous la forme d'un code de 3
caractères
 Les informations vous sont transmises au format D1;M;D2
La deuxième ligne contient un nombre entier N indiquant le nombre de taux de change qui vont vous
être transmis.
S'en suit N lignes représentant les taux de change représentés ainsi :
 La devise de départ DD sous la forme d'un code de 3 caractères
 La devise d'arrivée DA sous la forme d'un code de 3 caractères
 Le taux de change T sous la forme d'un nombre à 4 décimales (avec un "." comme séparateur
décimal)
 Les informations vous sont transmises au format DD;DA;T

Sortie du programme
Votre programme doit retourner le montant converti dans la devise cible arrondi à un nombre entier.

Exemple

Entrée
EUR;550;JPY
6
AUD;CHF;0.9661
JPY;KRW;13.1151
EUR;CHF;1.2053
AUD;JPY;86.0305
EUR;USD;1.2989
JPY;INR;0.6571

Sortie
59033

**************************************************


L'application est programmée en .NET 6.

Pour compiler le projet et retrouver l'exécutable dans le dossier courant exécutez la commande :

```shell
dotnet publish --configuration Release && cp ./sources/src/bin/Release/net6.0/LuccaDevises* .
LuccaDevises <chemin vers le fichier>```
```

Vous pouvez aussi directement passer par visual studio en ouvrant LuccaDevises.sln.

Le programme prend en 1er argument un chemin vers le fichier contenant les informations nécessaires à la conversion. L'argument est déjà paramatré par défaut avec le fichier test mais peut être modifié à votre guise

PS : le fichier test est dispo dans la solution : exampleLuccaDevise.txt et son chemin d'accès est déjà paramétré en argument lors du run de la solution
