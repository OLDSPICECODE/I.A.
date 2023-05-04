#define GLAD_GL_IMPLEMENTATION
#include <glad/gl.h>
#define GLFW_INCLUDE_NONE
#include <GLFW/glfw3.h>

#ifdef _WIN32
#include <Windows.h>
#else
#include <unistd.h>
#endif

#include "linmath.h"

#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>

#include <iostream>
#include <math.h>
#include <string>
#include <vector>

#include <algorithm>
#include <cmath>
#include <cstdlib>
#include <ctime>
#include <fstream>

float PI = 3.1415926f;
int n = 3600;
using namespace std;

vector<double> prom_p_gen;

// Estructura para almacenar las coordenadas de una ciudad
struct city {
  int x;
  int y;
};

// Función para calcular la distancia entre dos ciudades
double distance(city a, city b) {
  return sqrt(pow(a.x - b.x, 2) + pow(a.y - b.y, 2));
}

// Clase para representar una solución (ruta) del problema del agente viajero
class solution {
public:
  vector<int> route;
  double aptitud;

  // Constructor para generar una solución aleatoria
  solution(int num_cities) {
    for (int i = 0; i < num_cities; i++) {
      route.push_back(i);
    }
    random_shuffle(route.begin(), route.end());
    aptitud = 0;
  }

  // Constructor para crear una solución a partir de una ruta dada
  solution(vector<int> r) {
    route = r;
    aptitud = 0;
  }

  // Función para calcular la aptitud de la solución en términos de la distancia
  // total recorrida
  void evaluate(vector<city> cities) {
    aptitud = 0;
    for (int i = 0; i < route.size() - 1; i++) {
      int city_a = route[i];
      int city_b = route[i + 1];
      aptitud += distance(cities[city_a], cities[city_b]);
    }
    aptitud += distance(cities[route.back()], cities[route.front()]);
  }

  // Función para imprimir la ruta y la aptitud de la solución
  void print() {
    cout << "Route: ";
    for (int i = 0; i < route.size(); i++) {
      cout << route[i] << " ";
    }
    cout << endl;
    cout << "aptitud: " << aptitud << endl;
  }
};

// Función para realizar la selección de padres utilizando el método de
// selección por torneo
vector<solution> tournament_selection(vector<solution> population,
                                      int num_parents) {
  vector<solution> parents;
  for (int i = 0; i < num_parents; i++) {
    int idx_a = rand() % population.size();
    int idx_b = rand() % population.size();
    while (idx_b == idx_a) {
      idx_b = rand() % population.size();
    }
    if (population[idx_a].aptitud < population[idx_b].aptitud) {
      parents.push_back(population[idx_a]);
    } else {
      parents.push_back(population[idx_b]);
    }
  }
  return parents;
}

// Función para realizar el cruce en un punto entre dos padres
solution crossover(solution parent_a, solution parent_b) {
  int idx = rand() % parent_a.route.size();
  vector<int> child_route;
  for (int i = 0; i < idx; i++) {
    child_route.push_back(parent_a.route[i]);
  }
  for (int i = idx; i < parent_b.route.size(); i++) {
    if (find(child_route.begin(), child_route.end(), parent_b.route[i]) ==
        child_route.end()) {
      child_route.push_back(parent_b.route[i]);
    }
  }
  for (int i = 0; i < parent_a.route.size(); i++) {
    if (find(child_route.begin(), child_route.end(), parent_a.route[i]) ==
        child_route.end()) {
      child_route.push_back(parent_a.route[i]);
    }
  }
  return solution(child_route);
}

vector<solution> mejor;

// Función para realizar la mutación de una solución
void mutate(solution &s) {
  int idx_a = rand() % s.route.size();
  int idx_b = rand() % s.route.size();
  while (idx_b == idx_a) {
    idx_b = rand() % s.route.size();
  }
  swap(s.route[idx_a], s.route[idx_b]);
}

// Función para realizar una iteración del algoritmo genético
void framebuffer_size_callback(GLFWwindow *window, int width, int height);
void processInput(GLFWwindow *window);

void framebuffer_size_callback(GLFWwindow *window, int width, int height);
void processInput(GLFWwindow *window);

// settings
const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 600;

const char *vertexShaderSource =
    "#version 330 core\n"
    "layout (location = 0) in vec3 aPos;\n"
    "void main()\n"
    "{\n"
    "   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n"
    "}\0";
const char *fragmentShaderSource =
    "#version 330 core\n"
    "out vec4 FragColor;\n"
    "void main()\n"
    "{\n"
    "   FragColor = vec4(1.0f, 0.2f,0.4f,1.0f);\n"
    "}\n\0";

const char *vertexShaderSource2 =
    "#version 330 core\n"
    "layout (location = 0) in vec3 aPos;\n"
    "void main()\n"
    "{\n"
    "   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n"
    "}\0";
const char *fragmentShaderSource2 =
    "#version 330 core\n"
    "out vec4 FragColor;\n"
    "void main()\n"
    "{\n"
    "   FragColor = vec4(0.0f, 0.0f,0.0f,1.0f);\n"
    "}\n\0";

const char *vertexShaderSource3 =
    "#version 330 core\n"
    "layout (location = 0) in vec3 aPos;\n"
    "void main()\n"
    "{\n"
    "   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n"
    "}\0";
const char *fragmentShaderSource3 =
    "#version 330 core\n"
    "out vec4 FragColor;\n"
    "void main()\n"
    "{\n"
    "   FragColor = vec4(0.0f, 1.0f,1.0f,1.0f);\n"
    "}\n\0";

int main() {
  glfwInit();
  glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
  glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
  glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef __APPLE__
  glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
#endif

  // glfw window creation
  // --------------------
  GLFWwindow *window =
      glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearnOpenGL", NULL, NULL);
  if (window == NULL) {
    std::cout << "Failed to create GLFW window" << std::endl;
    glfwTerminate();
    return -1;
  }
  glfwMakeContextCurrent(window);
  glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

  gladLoadGL(glfwGetProcAddress);
  // build and compile our shader program
  // ------------------------------------
  // vertex shader
  unsigned int vertexShader = glCreateShader(GL_VERTEX_SHADER);
  glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
  glCompileShader(vertexShader);
  unsigned int vertexShader2 = glCreateShader(GL_VERTEX_SHADER);
  glShaderSource(vertexShader2, 1, &vertexShaderSource2, NULL);
  glCompileShader(vertexShader2);
  unsigned int vertexShader3 = glCreateShader(GL_VERTEX_SHADER);
  glShaderSource(vertexShader3, 1, &vertexShaderSource3, NULL);
  glCompileShader(vertexShader3);
  // check for shader compile errors
  int success;
  char infoLog[512];
  glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
  glGetShaderiv(vertexShader2, GL_COMPILE_STATUS, &success);
  glGetShaderiv(vertexShader3, GL_COMPILE_STATUS, &success);

  if (!success) {
    glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
    glGetShaderInfoLog(vertexShader2, 512, NULL, infoLog);
    glGetShaderInfoLog(vertexShader3, 512, NULL, infoLog);
    std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n"
              << infoLog << std::endl;
  }
  // fragment shader
  unsigned int fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
  glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
  glCompileShader(fragmentShader);
  unsigned int fragmentShader2 = glCreateShader(GL_FRAGMENT_SHADER);
  glShaderSource(fragmentShader2, 1, &fragmentShaderSource2, NULL);
  glCompileShader(fragmentShader2);
  unsigned int fragmentShader3 = glCreateShader(GL_FRAGMENT_SHADER);
  glShaderSource(fragmentShader3, 1, &fragmentShaderSource3, NULL);
  glCompileShader(fragmentShader3);
  // check for shader compile errors
  glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
  if (!success) {
    glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
    std::cout << "ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n"
              << infoLog << std::endl;
  }
  // link shaders
  unsigned int shaderProgram = glCreateProgram();
  glAttachShader(shaderProgram, vertexShader);
  glAttachShader(shaderProgram, fragmentShader);
  glLinkProgram(shaderProgram);
  unsigned int shaderProgram2 = glCreateProgram();
  glAttachShader(shaderProgram2, vertexShader2);
  glAttachShader(shaderProgram2, fragmentShader2);
  glLinkProgram(shaderProgram2);
  unsigned int shaderProgram3 = glCreateProgram();
  glAttachShader(shaderProgram3, vertexShader3);
  glAttachShader(shaderProgram3, fragmentShader3);
  glLinkProgram(shaderProgram3);
  // check for linking errors
  glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
  glGetProgramiv(shaderProgram2, GL_LINK_STATUS, &success);
  if (!success) {
    glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
    glGetProgramInfoLog(shaderProgram2, 512, NULL, infoLog);
    std::cout << "ERROR::SHADER::PROGRAM::LINKING_FAILED\n"
              << infoLog << std::endl;
  }
  glDeleteShader(vertexShader);
  glDeleteShader(fragmentShader);

  glDeleteShader(vertexShader2);
  glDeleteShader(fragmentShader2);

  float vertices2[99];
  
  float vertices[] = {
	  0.0f, 0.0f, 0.0f,
	  0.1f, 0.3f, 0.0f,
	  0.2, 0.1f,0.0f,
	  0.3f, 0.4f, 0.0f,
	  0.4f, 0.2f, 0.0f, 
	  0.10f, 0.4f, 0.0f,
	  0.20f, 0.25f,0.0f,
      0.10f, -0.29f,0.0f,
	  0.24f, -0.12f, 0.0f,
	  0.11f, -0.4f,0.0f, 
	  -0.22f, -0.24f,0.0f, 
	  -0.023f, 0.40f,0.0f, 
	  -0.51f, 0.3f,0.0f,  
	  -0.20f, 0.1f,0.0f,
      -0.3f, 0.14f,0.0f,
	  -0.4f, -0.8f, 0.0f,  
	  0.10f, -0.40f, 0.0f, 
	  0.20f, -0.20f, 0.0f, 
	  -0.10f, -0.9f, 0.0f,  
	  -0.20f, 0.12f, 0.0f, 
	  -0.10f, -0.41f, 0.0f,
      -0.22f, -0.2f,0.0f,  
	  -0.90f, 0.30f,0.0f, 
	  -0.10f, -0.30f,0.0f, 
	  0.2f, -0.10f,0.0f,  
	  0.3f, -0.40f,0.0f,   
	  0.40f, -0.2f,0.0f, 
	  0.10f, 0.44f,0.0f,
      -0.21f, 0.23f,0.0f,
	   -0.12f, 0.9f,0.0f,
	  0.20f, -0.9f,0.0f,
	  0.5f,-0.23f,0.0f,
	  -0.22f, 0.27f,0.0f,
  };
  unsigned int VBO, VAO;
  glGenVertexArrays(1, &VAO);
  glGenBuffers(1, &VBO);
  unsigned int VBO2, VAO2;
  glGenVertexArrays(1, &VAO2);
  glGenBuffers(1, &VBO2);
  // bind the Vertex Array Object first, then bind and set vertex buffer(s), and
  // then configure vertex attributes(s).
  glBindVertexArray(VAO);

  glBindBuffer(GL_ARRAY_BUFFER, VBO);
  glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void *)0);
  glEnableVertexAttribArray(0);
  glBindVertexArray(0);

  glBindVertexArray(0);
  vector<city> cities = {
      {0, 0},   {1, 3},   {2, 1},   {3, 4},   {4, 2},    {10, 4},  {20, 25},
      {10, 29}, {24, 12}, {11, 4},  {22, 24}, {023, 40}, {51, 3},  {20, 1},
      {3, 14},  {4, 8},   {10, 40}, {20, 20}, {10, 9},   {20, 12}, {10, 41},
      {22, 2},  {90, 30}, {10, 30}, {2, 10},  {3, 40},   {40, 2},  {10, 44},
      {21, 23}, {12, 9},  {20, 9},  {5, 23},  {22, 27}};

  // Configurar los parámetros del algoritmo genético
  int population_size = 100;
  int num_parents = 10;
  int num_iterations = 100; // generaciones

  // Inicializar la semilla aleatoria
  srand(time(NULL));
  // Ejecutar el algoritmo genético.
  // fstream initial

  // Generar la población inicial
  vector<solution> population;
  for (int i = 0; i < population_size; i++) {
    population.push_back(solution(cities.size()));
  }

  // Realizar el bucle principal del algoritmo genético
	for (int j = 0; j < population.size(); j++) {
		population[j].evaluate(cities);
	}

  double aux_prom_gen = 0;
  for (int k = 0; k < population.size(); k++) {
	aux_prom_gen += population[k].aptitud;
  }
  prom_p_gen.push_back(aux_prom_gen / population.size());
  // Seleccionar los padres para la siguiente generación
  vector<solution> parents = tournament_selection(population, num_parents);

  // Generar la siguiente generación mediante el cruce y la mutación de los
  // padres

  vector<solution> new_population;
  for (int j = 0; j < num_parents; j++) {
	solution parent_a = parents[j];
	solution parent_b = parents[(j + 1) % num_parents];
	solution child = crossover(parent_a, parent_b);
	if (rand() / (double)RAND_MAX < 0.1) {
	  mutate(child);
	}
	new_population.push_back(child);
  }

  // Reemplazar la población anterior con la nueva población
  population = new_population;

  // Imprimir la mejor solución de la iteración actual
  sort(population.begin(), population.end(),
	   [](solution a, solution b) { return a.aptitud < b.aptitud; });
  cout << "Iteration " << 0 + 1 << ": ";
  population[0].evaluate(cities);
  population[0].print();
  mejor.push_back(population[0]);
  cout<<mejor.size();
  int j=0;
  for(int i=0;i<mejor[0].route.size();i++){
	 vertices2[j] = float(vertices[mejor[0].route[i]*3]);
     vertices2[j+1] = float(vertices[mejor[0].route[i]*3+1]);
     vertices2[j+2] = 0.0f;
	 j+=3;
  }
  for(int i=0;i<99;i++){
	cout<<vertices2[i]<<endl;
  }
  glBindVertexArray(VAO2);
  glBindBuffer(GL_ARRAY_BUFFER, VBO2);
  glBufferData(GL_ARRAY_BUFFER, sizeof(vertices2), vertices2, GL_STATIC_DRAW);

  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void *)0);
  glEnableVertexAttribArray(0);
  glBindBuffer(GL_ARRAY_BUFFER, 0);
int i=1;
  while (!glfwWindowShouldClose(window)) {
    // input
    // -----
    processInput(window);

	// render
    // ------
    glClearColor(0.0f, 0.6f, 0.6f, 0.1f);
    glClear(GL_COLOR_BUFFER_BIT);
    glLineWidth(5);
    glUseProgram(shaderProgram);
	glPointSize(5);
    glBindVertexArray(
        VAO); // seeing as we only have a single VAO there's no need to bind it
              // every time, but we'll do so to keep things a bit more organized
    glDrawArrays(GL_POINTS, 0, 33);
    // glBindVertexArray(0); // no need to unbind it every time
    // glfw: swap buffers and poll IO events (keys pressed/released, mouse moved
    // etc.)
    // -------------------------------------------------------------------------------

    glUseProgram(shaderProgram2);
    glBindVertexArray(VAO2); // seeing as we only have a single VAO there's no
                             // need to bind it every time, but we'll do so to
                             // keep things a bit more organized
    glLineWidth(3);
    glDrawArrays(GL_LINE_LOOP, 0, 99);
    // draw our first triangle

    glUseProgram(shaderProgram3);
    glBindVertexArray(VAO2); // seeing as we only have a single VAO there's no
                             // need to bind it every time, but we'll do so to
                             // keep things a bit more organized
    glPointSize(10);
    glDrawArrays(GL_POINTS, 0, 10);

    glfwSwapBuffers(window);
    glfwPollEvents();
	if(i<num_iterations){
      // Evaluar la aptitud de la población actual
      for (int j = 0; j < population.size(); j++) {
        population[j].evaluate(cities);
      }

      double aux_prom_gen = 0;
      for (int k = 0; k < population.size(); k++) {
        aux_prom_gen += population[k].aptitud;
      }
      prom_p_gen.push_back(aux_prom_gen / population.size());
      // Seleccionar los padres para la siguiente generación
      vector<solution> parents = tournament_selection(population, num_parents);

      // Generar la siguiente generación mediante el cruce y la mutación de los
      // padres

      vector<solution> new_population;
      for (int j = 0; j < num_parents; j++) {
        solution parent_a = parents[j];
        solution parent_b = parents[(j + 1) % num_parents];
        solution child = crossover(parent_a, parent_b);
        if (rand() / (double)RAND_MAX < 0.1) {
          mutate(child);
        }
        new_population.push_back(child);
      }

      // Reemplazar la población anterior con la nueva población
      population = new_population;

      // Imprimir la mejor solución de la iteración actual
      sort(population.begin(), population.end(),
           [](solution a, solution b) { return a.aptitud < b.aptitud; });
      cout << "Iteration " << i + 1 << ": ";
      population[0].evaluate(cities);
      population[0].print();
      mejor.push_back(population[0]);
		j =0;
		for(int k=0;k<mejor[i].route.size();k++){
		 vertices2[j] = float(vertices[mejor[i].route[k]*3]);
		vertices2[j+1] = float(vertices[mejor[i].route[k]*3+1]);
		vertices2[j+2] = 0.0f;
		 j+=3;
		}
	}
  i+=1;
	glBindVertexArray(VAO2);
  glBindBuffer(GL_ARRAY_BUFFER, VBO2);
  glBufferData(GL_ARRAY_BUFFER, sizeof(vertices2), vertices2, GL_STATIC_DRAW);

  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void *)0);
  glEnableVertexAttribArray(0);
  glBindBuffer(GL_ARRAY_BUFFER, 0);
  Sleep(10);
  }
	ofstream salida("resultados.txt");
	if(salida.is_open()){  
	for (int i = 0; i < num_iterations; i++) {
		cout << "Promedio de la iteracion " << i + 1 << ": ";
		cout << prom_p_gen[i] << endl;
		salida << prom_p_gen[i] << endl;
		cout << "mejor de la iteracion " << i + 1 << ": ";
		mejor[i].print();
	  }	
	}
  // optional: de-allocate all resources once they've outlived their purpose:
  // ------------------------------------------------------------------------
  glDeleteVertexArrays(1, &VAO);
  glDeleteBuffers(1, &VBO);
  glDeleteProgram(shaderProgram);

  // glfw: terminate, clearing all previously allocated GLFW resources.
  // ------------------------------------------------------------------
  glfwTerminate();
  return 0;
}

// process all input: query GLFW whether relevant keys are pressed/released this
// frame and react accordingly
// ---------------------------------------------------------------------------------------------------------
void processInput(GLFWwindow *window) {
  if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
    glfwSetWindowShouldClose(window, true);
}

// glfw: whenever the window size changed (by OS or user resize) this callback
// function executes
// ---------------------------------------------------------------------------------------------
void framebuffer_size_callback(GLFWwindow *window, int width, int height) {
  // make sure the viewport matches the new window dimensions; note that width
  // and height will be significantly larger than specified on retina displays.
  glViewport(0, 0, width, height);
}
