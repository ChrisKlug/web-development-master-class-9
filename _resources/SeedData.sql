IF NOT EXISTS (SELECT * FROM Products WHERE Id = 1)
BEGIN
	INSERT INTO Products ([Name],[Description],Price,IsFeatured,ThumbnailUrl,ImageUrl) VALUES 
		('Banana', 'Nice yellow bananas from a warm country',0.99,1,'/images/products/banana_thumbnail.jpg','/images/products/banana.jpg'),
		('Orange', 'Orange fruit from a tree',1.29,1,'/images/products/orange_thumbnail.jpg','/images/products/orange.jpg'),
		('Apple', 'Keep the doctor away with these apples',0.49,1,'/images/products/apple_thumbnail.jpg','/images/products/apple.jpg'),
		('Kiwi', 'The fruit, not the bird',2.19,1,'/images/products/kiwi_thumbnail.jpg','/images/products/kiwi.jpg'),
		('Dragon Fruit', 'Oh yeah! There be dragons here...',1.79,0,'/images/products/dragon-fruit_thumbnail.jpg','/images/products/dragon-fruit.jpg'),
		('Durian', 'The worst smelling fruit in the world!',0.99,0,'/images/products/durian_thumbnail.jpg','/images/products/durian.jpg')
END