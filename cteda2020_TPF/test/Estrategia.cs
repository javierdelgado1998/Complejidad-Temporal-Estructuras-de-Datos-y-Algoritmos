
using System;
using System.Collections.Generic;
namespace DeepSpace
{
	class Estrategia
	{
		public String Consulta1( ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>(); //Inicializamos la cola
			ArbolGeneral<Planeta> aux;	//Declaramos la variable auxiliar
			int nivel = 0;				//En esta variable guardamos el nivel en donde nos encontremos
			c.encolar(arbol);			//Encolamos el arbol
			c.encolar(null);			//Encolamos el separador
			while(!c.esVacia())
			{
				aux = c.desencolar();	
				if(aux != null)
				{
					if(aux.getDatoRaiz() != null)
					{
						if(aux.getDatoRaiz().EsPlanetaDeLaIA()) //Si el planeta encontrado pertenece a la IA, retornamos el nivel actual
						{
							return "Distancia de raiz al planeta mas cercano(bot): "+nivel;
						}
					}
				}
				else
				{
					nivel++;
					if(!c.esVacia())
					{
						c.encolar(null);
					}
					foreach (var hijo in aux.getHijos())
					{
						c.encolar(hijo);
					}
				}
				/*if(aux == null) // En caso de que encontramos el separador, aumentamos el nivel
				{
					nivel++;
					if(!c.esVacia())
					{
						c.encolar(null);
					}
				}
				else	// Encolamos el resto de los hijos
				{
					foreach (var hijo in aux.getHijos())
					{
						c.encolar(hijo);
					}
				}*/
			}
			return null;
		}


		public String Consulta2( ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> aux;
			int nivel = 0;
			int cantidadPlanetas = 0;	//Guardamos la cantidad de planetas por nivel, con población mayor a 10
			string consulta2 = null;
			c.encolar(arbol);
			c.encolar(null);
			while(!c.esVacia())
			{
				aux = c.desencolar();
				if(aux != null)
				{
					if(aux.getDatoRaiz() != null)
					{
						if(aux.getDatoRaiz().Poblacion() > 10) //Si la poblacion supera las 10 unidades, aumentamos la cantidad de planetas
						{
							cantidadPlanetas++;
						}
					}
				}
				if(aux == null)	//Si encontramos el separador, agregamos la información obtenida del nivel, y reiniciamos las variables.
				{
					consulta2+= "Nivel: "+nivel+" Planetas: "+cantidadPlanetas+" ";
					nivel++;
					cantidadPlanetas = 0;
					if(!c.esVacia())
					{
						c.encolar(null);
					}
				}
				else
				{
					foreach (var hijo in aux.getHijos())
					{
						c.encolar(hijo);
					}
				}
			}
			return consulta2;
		}


		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> aux;
			int nivel = 0;
			int cantidadPlanetas = 0;	//Guardamos la cantidad de planetas por nivel
			int poblacion = 0;			//Guardamos la poblacion de cada planeta
			string consulta3 = null;
			c.encolar(arbol);
			c.encolar(null);
			while(!c.esVacia())
			{
				aux = c.desencolar();
				
				if(aux != null)
				{
					if(aux.getDatoRaiz() != null)	//Acumulamos la suma de poblaciones y aumentamos la cantidad de planetas
					{
						poblacion += aux.getDatoRaiz().Poblacion();
						cantidadPlanetas++;
					}
				}
				if(aux == null) //Finalizando el recorrido del nivel, sacamos el promedio, guardamos, y reinciamos las variables
				{
					float promedioPorNivel = (float)poblacion/cantidadPlanetas;
					consulta3+= "\nNivel :"+nivel+" Promedio poblacional: "+promedioPorNivel;
					nivel++;
					cantidadPlanetas = 0;
					poblacion = 0;
					if(!c.esVacia())
					{
						c.encolar(null);
					}
				}
				else
				{
					foreach (var hijo in aux.getHijos())
					{
						c.encolar(hijo);
					}
				}
			}
			return consulta3;
		}
		
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			//Si la raiz no esta ocupada por la IA, entonces formara un camino hasta conquistarla.
			if(!arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
				List<Planeta> mov = caminoHastaIA(arbol);
				for(int i = mov.Count-1; i > 0; i--)
				{
					if(mov[i].EsPlanetaDeLaIA()) //Los movimientos son desde la posicion donde se encuntra la IA hasta la raiz
					{
						return new Movimiento(mov[i],mov[i-1]);
					}
				}
			}
			/*Una vez conquistada la raiz, la IA solo se dedica a formar caminos hacia el jugador, atacandolo hasta que pierda,
  			 donde los movimientos ahora serian en sentido opuesto... */
			else
			{
				List<Planeta> movAtaque = caminoHastaJugador(arbol);
				for(int i = 0 ; i < movAtaque.Count-1; i++)
				{
					/*Si la raiz fue conquistada, la primera posicion corresponderia a la IA, por lo cual los movimientos
					 siguientes serian una posicion mas adelante*/
					if(!movAtaque[i+1].EsPlanetaDeLaIA())//Los movimientos son desde la raiz hasta el jugador humano
					{
						return new Movimiento(movAtaque[i],movAtaque[i+1]);
					}
				}
			}

			return null;
		}
		
		public List<Planeta> caminoHastaIA(ArbolGeneral<Planeta> arbol)
		{
			List<Planeta> camino = new List<Planeta>();
			return _caminoHastaIA(arbol,camino);
		}
		
		private List<Planeta> _caminoHastaIA(ArbolGeneral<Planeta> arbol, List<Planeta> camino)
		{
			//Primero raiz...
			camino.Add(arbol.getDatoRaiz());
			
			//Si encontramos camino...
			if(arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
				return camino;
			}
			else
			{
				//Hijos recursivamente...
				foreach(var hijo in arbol.getHijos())
				{
					List<Planeta> caminoAux = _caminoHastaIA(hijo, camino);
					if(caminoAux != null)
					{
						return caminoAux;
					}	
				}
				//Saco ultimo planeta de camino
				camino.RemoveAt(camino.Count-1);
			}
			return null;
		}
		
		public List<Planeta> caminoHastaJugador(ArbolGeneral<Planeta> arbol)
		{
			List<Planeta> camino = new List<Planeta>();
			return _caminoHastaJugador(arbol,camino);
		}
		
		private List<Planeta> _caminoHastaJugador(ArbolGeneral<Planeta> arbol, List<Planeta> camino)
		{
			camino.Add(arbol.getDatoRaiz());
			if(arbol.getDatoRaiz().EsPlanetaDelJugador()) //Condicion que cambia con respecto al metodo anterior
			{
				return camino;
			}
			else
			{
				foreach(var hijo in arbol.getHijos())
				{
					List<Planeta> caminoAux = _caminoHastaJugador(hijo, camino);
					if(caminoAux != null)
					{
						return caminoAux;
					}	
				}
				camino.RemoveAt(camino.Count-1);
			}
			return null;
		}
	}
}
