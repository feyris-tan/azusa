-- --------------------------------------------------------
-- Host:                         192.168.0.3
-- Server Version:               5.5.59-MariaDB - Source distribution
-- Server Betriebssystem:        Linux
-- HeidiSQL Version:             9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Exportiere Datenbank Struktur für mediaLibrary
CREATE DATABASE IF NOT EXISTS `mediaLibrary` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `mediaLibrary`;

-- Exportiere Struktur von Tabelle mediaLibrary.osm_nodes
CREATE TABLE IF NOT EXISTS `osm_nodes` (
  `id` bigint(20) NOT NULL,
  `lat` float NOT NULL,
  `lon` float NOT NULL,
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `nodes_lat_index` (`lat`),
  KEY `nodes_lon_index` (`lon`),
  KEY `nodes_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle mediaLibrary.osm_node_tags
CREATE TABLE IF NOT EXISTS `osm_node_tags` (
  `nodeId` bigint(20) NOT NULL,
  `k` varchar(128) NOT NULL,
  `v` varchar(128) DEFAULT NULL,
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`nodeId`,`k`),
  KEY `nodetags_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle mediaLibrary.osm_relations
CREATE TABLE IF NOT EXISTS `osm_relations` (
  `id` bigint(20) NOT NULL,
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `relations_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle mediaLibrary.osm_relation_member
CREATE TABLE IF NOT EXISTS `osm_relation_member` (
  `relationId` bigint(20) NOT NULL,
  `type` varchar(10) NOT NULL,
  `reference` bigint(20) NOT NULL,
  `role` varchar(10) DEFAULT NULL,
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  KEY `relation_member_relationId_index` (`relationId`),
  KEY `relation_member_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle mediaLibrary.osm_relation_tags
CREATE TABLE IF NOT EXISTS `osm_relation_tags` (
  `relationId` bigint(20) NOT NULL,
  `k` varchar(128) NOT NULL,
  `v` varchar(128) DEFAULT 'NULL',
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`relationId`,`k`),
  KEY `relation_tags_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle mediaLibrary.osm_ways
CREATE TABLE IF NOT EXISTS `osm_ways` (
  `id` bigint(20) NOT NULL,
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `ways_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle mediaLibrary.osm_way_nodes
CREATE TABLE IF NOT EXISTS `osm_way_nodes` (
  `way` bigint(20) NOT NULL,
  `node` bigint(20) NOT NULL,
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`way`,`node`),
  KEY `way_nodes_way_index` (`way`),
  KEY `way_nodes_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
-- Exportiere Struktur von Tabelle mediaLibrary.osm_way_tags
CREATE TABLE IF NOT EXISTS `osm_way_tags` (
  `wayId` bigint(20) NOT NULL,
  `k` varchar(128) NOT NULL,
  `v` varchar(128) DEFAULT 'NULL',
  `dateAdded` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`wayId`,`k`),
  KEY `way_tags_dateAdded_index` (`dateAdded`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Daten Export vom Benutzer nicht ausgewählt
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
