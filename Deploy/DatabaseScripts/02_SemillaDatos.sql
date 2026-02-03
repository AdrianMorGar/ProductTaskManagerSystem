-- Insertar Productos de Ejemplo
INSERT INTO Productos (Nombre, Precio, Stock, Categoria) VALUES 
('Portátil Developer X1', 1250.00, 10, 'Informática'),
('Monitor 4K Ultra', 350.50, 25, 'Periféricos'),
('Teclado Mecánico', 89.99, 50, 'Periféricos'),
('Silla Ergonómica', 199.00, 5, 'Mobiliario');

-- Tareas para el Portátil (ID 1)
INSERT INTO Tareas (Descripcion, EstaCompletada, ProductoId) VALUES 
('Instalar Visual Studio 2022', 0, 1),
('Actualizar Drivers Gráficos', 1, 1),
('Configurar IP Estática', 0, 1);

-- Tareas para la Silla (ID 4)
INSERT INTO Tareas (Descripcion, EstaCompletada, ProductoId) VALUES 
('Apretar tornillos base', 0, 4),
('Limpiar tapicería', 0, 4);