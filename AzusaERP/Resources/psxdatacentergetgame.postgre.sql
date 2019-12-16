SELECT game.platform, game.sku,game.title,game.languages,game.commontitle,game.region, genre.name, 
	   developer.name, publisher.name, game.daterelease, game.cover, game.description, game.barcode
FROM dump_psxdatacenter.games game
JOIN dump_psxdatacenter.genres genre ON game.genreid = genre.id
JOIN dump_psxdatacenter.companies developer ON game.developerid = developer.id
JOIN dump_psxdatacenter.companies publisher ON game.publisherid = publisher.id
WHERE game.id=@id