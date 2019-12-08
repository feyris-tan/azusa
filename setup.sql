--
-- PostgreSQL database dump
--

-- Dumped from database version 12.1 (Debian 12.1-1.pgdg100+1)
-- Dumped by pg_dump version 12.1 (Debian 12.1-1.pgdg90+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: azusa_countries; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_countries (
    id integer NOT NULL,
    "displayName" character varying(100) NOT NULL,
    "BTS" character varying(10) NOT NULL,
    currency character varying(10) NOT NULL,
    conversion double precision,
    language integer,
    "singlePhaseVoltage" smallint,
    "voltageFrequency" smallint,
    "dvdRegion" smallint,
    "blurayRegion" character(1),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.azusa_countries OWNER TO ft;

--
-- Name: azusa_countries_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.azusa_countries_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.azusa_countries_id_seq OWNER TO ft;

--
-- Name: azusa_countries_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.azusa_countries_id_seq OWNED BY public.azusa_countries.id;


--
-- Name: azusa_dumpMedia_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public."azusa_dumpMedia_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."azusa_dumpMedia_id_seq" OWNER TO ft;

--
-- Name: azusa_filesysteminfo; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_filesysteminfo (
    id bigint NOT NULL,
    mediaid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    isdirectory boolean NOT NULL,
    fullname character varying(255) NOT NULL,
    size bigint,
    modified timestamp without time zone,
    head bytea,
    parent bigint NOT NULL
);


ALTER TABLE public.azusa_filesysteminfo OWNER TO ft;

--
-- Name: azusa_filesysteminfo_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.azusa_filesysteminfo_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.azusa_filesysteminfo_id_seq OWNER TO ft;

--
-- Name: azusa_filesysteminfo_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.azusa_filesysteminfo_id_seq OWNED BY public.azusa_filesysteminfo.id;


--
-- Name: azusa_languages; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_languages (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    iso639_1 character varying(2),
    iso639_2 character varying(3),
    iso639_3 character varying(3) NOT NULL,
    iso639_5 character varying(3) NOT NULL,
    "isoName" character varying(50) NOT NULL,
    scope character varying(50) NOT NULL,
    type character varying(50) NOT NULL,
    "nativeName" character varying(50) NOT NULL,
    "otherName" character varying(50) NOT NULL,
    family character varying(50) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.azusa_languages OWNER TO ft;

--
-- Name: azusa_languages_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.azusa_languages_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.azusa_languages_id_seq OWNER TO ft;

--
-- Name: azusa_languages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.azusa_languages_id_seq OWNED BY public.azusa_languages.id;


--
-- Name: azusa_media; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_media (
    id integer NOT NULL,
    relatedproduct integer NOT NULL,
    name character varying(128) NOT NULL,
    mediatypeid integer NOT NULL,
    sku character varying(50),
    dumpstoragespace integer,
    dumppath character varying(255),
    metafile text,
    dateadded timestamp without time zone,
    graphdata text,
    untouchedcuesheet text,
    untouchedchecksum text,
    untouchedplaylist text,
    cdtext bytea,
    logfile text,
    mediadescriptorsidecar bytea,
    issealed boolean NOT NULL,
    dateupdated timestamp without time zone,
    fauxhash bigint
);


ALTER TABLE public.azusa_media OWNER TO ft;

--
-- Name: azusa_mediaTypes_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public."azusa_mediaTypes_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."azusa_mediaTypes_id_seq" OWNER TO ft;

--
-- Name: azusa_mediaTypes_id_seqB; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public."azusa_mediaTypes_id_seqB"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."azusa_mediaTypes_id_seqB" OWNER TO ft;

--
-- Name: azusa_media_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.azusa_media_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.azusa_media_id_seq OWNER TO ft;

--
-- Name: azusa_media_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.azusa_media_id_seq OWNED BY public.azusa_media.id;


--
-- Name: azusa_mediatypes; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_mediatypes (
    id integer NOT NULL,
    "shortName" character varying(25) NOT NULL,
    "longName" character varying(255),
    graphdata boolean,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    icon bytea NOT NULL,
    "ignoreForStatistics" boolean NOT NULL,
    vndbkey character varying(5)
);


ALTER TABLE public.azusa_mediatypes OWNER TO ft;

--
-- Name: azusa_platforms; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_platforms (
    id integer NOT NULL,
    "shortName" character varying(25) NOT NULL,
    "longName" character varying(200),
    "isSoftware" boolean NOT NULL,
    dateadded timestamp without time zone
);


ALTER TABLE public.azusa_platforms OWNER TO ft;

--
-- Name: azusa_platforms_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.azusa_platforms_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.azusa_platforms_id_seq OWNER TO ft;

--
-- Name: azusa_platforms_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.azusa_platforms_id_seq OWNED BY public.azusa_platforms.id;


--
-- Name: azusa_products; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_products (
    id integer NOT NULL,
    inshelf integer NOT NULL,
    name character varying(150) NOT NULL,
    picture bytea,
    price double precision,
    "boughtOn" date,
    sku character varying(50),
    platform integer,
    supplier integer,
    "countryOfOrigin" integer,
    screenshot bytea,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    consistent boolean NOT NULL,
    nsfw boolean,
    complete boolean NOT NULL,
    dateupdated timestamp without time zone
);


ALTER TABLE public.azusa_products OWNER TO ft;

--
-- Name: azusa_products_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.azusa_products_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.azusa_products_id_seq OWNER TO ft;

--
-- Name: azusa_products_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.azusa_products_id_seq OWNED BY public.azusa_products.id;


--
-- Name: azusa_shelves; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_shelves (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    "showSku" boolean NOT NULL,
    "showRegion" boolean,
    "showPlatform" boolean,
    "ignoreForStatistics" boolean NOT NULL,
    "screenshotRequired" boolean NOT NULL,
    dateadded timestamp without time zone
);


ALTER TABLE public.azusa_shelves OWNER TO ft;

--
-- Name: azusa_shops; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_shops (
    id integer NOT NULL,
    name character varying(200),
    "isPeriodicEvent" boolean NOT NULL,
    url character varying(255),
    dateadded timestamp without time zone
);


ALTER TABLE public.azusa_shops OWNER TO ft;

--
-- Name: azusa_statistics; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.azusa_statistics (
    date date NOT NULL,
    "totalProducts" integer,
    "totalMedia" integer,
    "missingCover" integer,
    "missingGraph" integer,
    undumped integer,
    "missingScreenshots" integer,
    dateadded timestamp without time zone
);


ALTER TABLE public.azusa_statistics OWNER TO ft;

--
-- Name: cddb_category; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cddb_category (
    id smallint NOT NULL,
    name character varying(16) NOT NULL,
    dateadded timestamp without time zone DEFAULT now()
);


ALTER TABLE public.cddb_category OWNER TO postgres;

--
-- Name: cddb_category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.cddb_category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.cddb_category_id_seq OWNER TO postgres;

--
-- Name: cddb_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.cddb_category_id_seq OWNED BY public.cddb_category.id;


--
-- Name: cddb_disc; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cddb_disc (
    category integer NOT NULL,
    discid character varying(8) NOT NULL,
    artist character varying(255),
    title character varying(255),
    year smallint,
    genre character varying(64),
    extradata character varying(255),
    playorder character varying(128),
    sqlid bigint NOT NULL,
    dateadded timestamp without time zone DEFAULT now()
);


ALTER TABLE public.cddb_disc OWNER TO postgres;

--
-- Name: cddb_disc_sqlid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.cddb_disc_sqlid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.cddb_disc_sqlid_seq OWNER TO postgres;

--
-- Name: cddb_disc_sqlid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.cddb_disc_sqlid_seq OWNED BY public.cddb_disc.sqlid;


--
-- Name: cddb_track; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cddb_track (
    discsqlid bigint NOT NULL,
    trackno smallint NOT NULL,
    dateadded timestamp without time zone DEFAULT now(),
    name character varying(128) NOT NULL,
    extradata character varying(128)
);


ALTER TABLE public.cddb_track OWNER TO postgres;

--
-- Name: dexcom_history; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dexcom_history (
    date date NOT NULL,
    "time" time without time zone NOT NULL,
    filtered integer,
    unfiltered integer,
    rssi integer,
    glucose integer,
    trend integer,
    "sessionState" integer,
    "meterGlucose" integer,
    "eventType" integer,
    carbs integer,
    insulin integer,
    "eventSubType" integer,
    "specialGlucoseValue" integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    dateupdated timestamp without time zone
);


ALTER TABLE public.dexcom_history OWNER TO ft;

--
-- Name: dexcom_manualdata; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dexcom_manualdata (
    pid bigint NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ts timestamp without time zone NOT NULL,
    messwert smallint NOT NULL,
    einheit character varying(10) NOT NULL,
    be smallint,
    novorapid smallint,
    levemir smallint,
    hide boolean NOT NULL,
    minutemodifier integer NOT NULL,
    remark character varying(255) NOT NULL
);


ALTER TABLE public.dexcom_manualdata OWNER TO ft;

--
-- Name: discarchivator_joblist; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.discarchivator_joblist (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    volumelabel character varying(128) NOT NULL,
    discid bigint NOT NULL,
    vts integer NOT NULL,
    pgc integer
);


ALTER TABLE public.discarchivator_joblist OWNER TO ft;

--
-- Name: discarchivator_joblist_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.discarchivator_joblist_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.discarchivator_joblist_id_seq OWNER TO ft;

--
-- Name: discarchivator_joblist_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.discarchivator_joblist_id_seq OWNED BY public.discarchivator_joblist.id;


--
-- Name: discarchivator_tasklog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.discarchivator_tasklog (
    id integer NOT NULL,
    finished timestamp without time zone DEFAULT now() NOT NULL,
    rhost character varying(32) DEFAULT inet_client_addr() NOT NULL,
    disc character varying(64),
    track character varying(24),
    exename character varying(64),
    timetaken integer,
    arguments text,
    workingdirectory character varying(255),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.discarchivator_tasklog OWNER TO postgres;

--
-- Name: discarchivator_tasklog_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.discarchivator_tasklog_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.discarchivator_tasklog_id_seq OWNER TO postgres;

--
-- Name: discarchivator_tasklog_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.discarchivator_tasklog_id_seq OWNED BY public.discarchivator_tasklog.id;


--
-- Name: dump_gb_posts; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_gb_posts (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    height integer,
    score integer,
    fileurl text,
    parentid text,
    sampleurl text,
    samplewidth integer,
    sampleheight integer,
    previewurl text,
    rating character varying(4),
    width integer,
    change integer,
    md5 character varying(48),
    creatorid integer,
    haschildren boolean DEFAULT false NOT NULL,
    createdat timestamp without time zone,
    status integer,
    source text,
    hasnotes boolean DEFAULT false NOT NULL,
    hascomments boolean DEFAULT false NOT NULL,
    previewwidth integer,
    previewheight integer,
    downloaded boolean DEFAULT false NOT NULL,
    skipdownload boolean DEFAULT false NOT NULL
);


ALTER TABLE public.dump_gb_posts OWNER TO ft;

--
-- Name: dump_gb_posttags; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_gb_posttags (
    postid integer NOT NULL,
    tagid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.dump_gb_posttags OWNER TO ft;

--
-- Name: dump_gb_tags; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_gb_tags (
    tag character varying(255) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false NOT NULL,
    id integer NOT NULL
);


ALTER TABLE public.dump_gb_tags OWNER TO ft;

--
-- Name: dump_gb_tags_seq_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_gb_tags_seq_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_gb_tags_seq_seq OWNER TO ft;

--
-- Name: dump_gb_tags_seq_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_gb_tags_seq_seq OWNED BY public.dump_gb_tags.id;


--
-- Name: dump_myfigurecollection_0dumpmeta; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_myfigurecollection_0dumpmeta (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    key1 character varying(64),
    key2 character varying(64),
    keyutime integer
);


ALTER TABLE public.dump_myfigurecollection_0dumpmeta OWNER TO ft;

--
-- Name: dump_myfigurecollection_0dumpmeta_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_myfigurecollection_0dumpmeta_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_myfigurecollection_0dumpmeta_id_seq OWNER TO ft;

--
-- Name: dump_myfigurecollection_0dumpmeta_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_myfigurecollection_0dumpmeta_id_seq OWNED BY public.dump_myfigurecollection_0dumpmeta.id;


--
-- Name: dump_myfigurecollection_0statistics; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_myfigurecollection_0statistics (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    numfigures integer NOT NULL
);


ALTER TABLE public.dump_myfigurecollection_0statistics OWNER TO ft;

--
-- Name: dump_myfigurecollection_0statistics_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_myfigurecollection_0statistics_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_myfigurecollection_0statistics_id_seq OWNER TO ft;

--
-- Name: dump_myfigurecollection_0statistics_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_myfigurecollection_0statistics_id_seq OWNED BY public.dump_myfigurecollection_0statistics.id;


--
-- Name: dump_myfigurecollection_categories; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_myfigurecollection_categories (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    originalid integer NOT NULL,
    name character varying(128) NOT NULL,
    color character varying(12) NOT NULL
);


ALTER TABLE public.dump_myfigurecollection_categories OWNER TO ft;

--
-- Name: dump_myfigurecollection_categories_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_myfigurecollection_categories_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_myfigurecollection_categories_id_seq OWNER TO ft;

--
-- Name: dump_myfigurecollection_categories_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_myfigurecollection_categories_id_seq OWNED BY public.dump_myfigurecollection_categories.id;


--
-- Name: dump_myfigurecollection_figurephotos; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_myfigurecollection_figurephotos (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    image bytea,
    thumbnail bytea
);


ALTER TABLE public.dump_myfigurecollection_figurephotos OWNER TO ft;

--
-- Name: dump_myfigurecollection_figures; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_myfigurecollection_figures (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    thumbnailurl character varying(255),
    fullurl character varying(255),
    rootid integer,
    categoryid integer,
    barcode character varying(32),
    name text,
    release_date date,
    price double precision,
    scraped boolean DEFAULT false NOT NULL,
    enabled boolean DEFAULT true NOT NULL
);


ALTER TABLE public.dump_myfigurecollection_figures OWNER TO ft;

--
-- Name: dump_myfigurecollection_roots; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_myfigurecollection_roots (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(128) NOT NULL,
    originalid integer NOT NULL
);


ALTER TABLE public.dump_myfigurecollection_roots OWNER TO ft;

--
-- Name: dump_myfigurecollection_roots_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_myfigurecollection_roots_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_myfigurecollection_roots_id_seq OWNER TO ft;

--
-- Name: dump_myfigurecollection_roots_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_myfigurecollection_roots_id_seq OWNED BY public.dump_myfigurecollection_roots.id;


--
-- Name: dump_psxdatacenter_companies; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_psxdatacenter_companies (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.dump_psxdatacenter_companies OWNER TO ft;

--
-- Name: dump_psxdatacenter_companies_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_psxdatacenter_companies_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_psxdatacenter_companies_id_seq OWNER TO ft;

--
-- Name: dump_psxdatacenter_companies_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_psxdatacenter_companies_id_seq OWNED BY public.dump_psxdatacenter_companies.id;


--
-- Name: dump_psxdatacenter_game_screenshots; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_psxdatacenter_game_screenshots (
    gameid integer NOT NULL,
    screenshotid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    hibernateid bigint NOT NULL
);


ALTER TABLE public.dump_psxdatacenter_game_screenshots OWNER TO ft;

--
-- Name: dump_psxdatacenter_game_screenshots_hibernateid_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_psxdatacenter_game_screenshots_hibernateid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_psxdatacenter_game_screenshots_hibernateid_seq OWNER TO ft;

--
-- Name: dump_psxdatacenter_game_screenshots_hibernateid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_psxdatacenter_game_screenshots_hibernateid_seq OWNED BY public.dump_psxdatacenter_game_screenshots.hibernateid;


--
-- Name: dump_psxdatacenter_games; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_psxdatacenter_games (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    platform character varying(4) NOT NULL,
    sku character varying(64) NOT NULL,
    title character varying(255) NOT NULL,
    languages character varying(128),
    additionals boolean NOT NULL,
    commontitle character varying(255),
    region character varying(8) NOT NULL,
    genreid integer,
    developerid integer,
    publisherid integer,
    daterelease date,
    cover bytea,
    description text,
    barcode character varying(16),
    scalerid integer
);


ALTER TABLE public.dump_psxdatacenter_games OWNER TO ft;

--
-- Name: dump_psxdatacenter_games_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_psxdatacenter_games_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_psxdatacenter_games_id_seq OWNER TO ft;

--
-- Name: dump_psxdatacenter_games_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_psxdatacenter_games_id_seq OWNED BY public.dump_psxdatacenter_games.id;


--
-- Name: dump_psxdatacenter_genres; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_psxdatacenter_genres (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.dump_psxdatacenter_genres OWNER TO ft;

--
-- Name: dump_psxdatacenter_genres_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_psxdatacenter_genres_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_psxdatacenter_genres_id_seq OWNER TO ft;

--
-- Name: dump_psxdatacenter_genres_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_psxdatacenter_genres_id_seq OWNED BY public.dump_psxdatacenter_genres.id;


--
-- Name: dump_psxdatacenter_screenshots; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_psxdatacenter_screenshots (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(255) NOT NULL,
    buffer bytea NOT NULL,
    scalerid integer
);


ALTER TABLE public.dump_psxdatacenter_screenshots OWNER TO ft;

--
-- Name: dump_psxdatacenter_screenshotdata_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_psxdatacenter_screenshotdata_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_psxdatacenter_screenshotdata_id_seq OWNER TO ft;

--
-- Name: dump_psxdatacenter_screenshotdata_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_psxdatacenter_screenshotdata_id_seq OWNED BY public.dump_psxdatacenter_screenshots.id;


--
-- Name: dump_vgmdb_0dumpmeta; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_0dumpmeta (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    key1 character varying(128) NOT NULL,
    key2 character varying(64),
    keyutime bigint
);


ALTER TABLE public.dump_vgmdb_0dumpmeta OWNER TO ft;

--
-- Name: dump_vgmdb_0errors; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_0errors (
    id integer NOT NULL,
    type character varying(16) NOT NULL,
    item_id integer NOT NULL,
    code integer NOT NULL,
    remark character varying(255),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_0errors OWNER TO ft;

--
-- Name: dump_vgmdb_0errors_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_0errors_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_0errors_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_0errors_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_0errors_id_seq OWNED BY public.dump_vgmdb_0errors.id;


--
-- Name: dump_vgmdb_0statistics; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_0statistics (
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    albumsdone integer,
    albumstotal integer,
    artistsdone integer,
    artiststotal integer
);


ALTER TABLE public.dump_vgmdb_0statistics OWNER TO ft;

--
-- Name: dump_vgmdb_album_arbituaryproducts; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_arbituaryproducts (
    albumid integer NOT NULL,
    ordinal integer NOT NULL,
    name character varying(384) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_arbituaryproducts OWNER TO ft;

--
-- Name: dump_vgmdb_album_artist_arbitrary; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_artist_arbitrary (
    albumid integer NOT NULL,
    artisttypeid integer NOT NULL,
    name character varying(128) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_artist_arbitrary OWNER TO ft;

--
-- Name: dump_vgmdb_album_artist_type; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_artist_type (
    id integer NOT NULL,
    name character varying(16) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_artist_type OWNER TO ft;

--
-- Name: dump_vgmdb_album_artist_type_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_album_artist_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_album_artist_type_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_album_artist_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_album_artist_type_id_seq OWNED BY public.dump_vgmdb_album_artist_type.id;


--
-- Name: dump_vgmdb_album_artists; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_artists (
    albumid integer NOT NULL,
    artistid integer NOT NULL,
    artisttypeid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_artists OWNER TO ft;

--
-- Name: dump_vgmdb_album_classification; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_classification (
    id integer NOT NULL,
    name character varying(128) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_classification OWNER TO ft;

--
-- Name: dump_vgmdb_album_classification_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_album_classification_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_album_classification_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_album_classification_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_album_classification_id_seq OWNED BY public.dump_vgmdb_album_classification.id;


--
-- Name: dump_vgmdb_album_cover; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_cover (
    albumid integer NOT NULL,
    covername character varying(64) NOT NULL,
    buffer bytea,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ordinal integer DEFAULT 0 NOT NULL,
    scalerid integer
);


ALTER TABLE public.dump_vgmdb_album_cover OWNER TO ft;

--
-- Name: dump_vgmdb_album_disc_track_translation; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_disc_track_translation (
    albumid integer NOT NULL,
    discindex integer NOT NULL,
    trackindex integer NOT NULL,
    lang character varying(64) NOT NULL,
    name character varying(384) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_disc_track_translation OWNER TO ft;

--
-- Name: dump_vgmdb_album_disc_tracks; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_disc_tracks (
    albumid integer NOT NULL,
    discindex integer NOT NULL,
    trackindex integer NOT NULL,
    tracklength integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_disc_tracks OWNER TO ft;

--
-- Name: dump_vgmdb_album_discs; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_discs (
    albumid integer NOT NULL,
    discindex integer NOT NULL,
    name character varying(16) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_discs OWNER TO ft;

--
-- Name: dump_vgmdb_album_label_arbiturary; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_label_arbiturary (
    albumid integer NOT NULL,
    ordinal integer NOT NULL,
    roleid integer NOT NULL,
    name character varying(255) NOT NULL,
    currenttimestamp timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.dump_vgmdb_album_label_arbiturary OWNER TO ft;

--
-- Name: dump_vgmdb_album_label_roles; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_label_roles (
    id integer NOT NULL,
    name character varying(32) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_label_roles OWNER TO ft;

--
-- Name: dump_vgmdb_album_label_roles_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_album_label_roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_album_label_roles_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_album_label_roles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_album_label_roles_id_seq OWNED BY public.dump_vgmdb_album_label_roles.id;


--
-- Name: dump_vgmdb_album_labels; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_labels (
    albumid integer NOT NULL,
    labelid integer NOT NULL,
    roleid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.dump_vgmdb_album_labels OWNER TO ft;

--
-- Name: dump_vgmdb_album_mediaformat; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_mediaformat (
    id integer NOT NULL,
    name character varying(64) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_mediaformat OWNER TO ft;

--
-- Name: dump_vgmdb_album_mediaformat_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_album_mediaformat_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_album_mediaformat_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_album_mediaformat_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_album_mediaformat_id_seq OWNED BY public.dump_vgmdb_album_mediaformat.id;


--
-- Name: dump_vgmdb_album_relatedalbum; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_relatedalbum (
    albumid integer NOT NULL,
    relatedalbumid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.dump_vgmdb_album_relatedalbum OWNER TO ft;

--
-- Name: dump_vgmdb_album_releaseevent; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_releaseevent (
    albumid integer NOT NULL,
    eventid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_releaseevent OWNER TO ft;

--
-- Name: dump_vgmdb_album_reprints; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_reprints (
    albumid integer NOT NULL,
    reprintid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_reprints OWNER TO ft;

--
-- Name: dump_vgmdb_album_titles; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_titles (
    albumid integer NOT NULL,
    langname character varying(7) NOT NULL,
    title character varying(384) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_titles OWNER TO ft;

--
-- Name: dump_vgmdb_album_types; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_types (
    id smallint NOT NULL,
    name character varying(32) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_types OWNER TO ft;

--
-- Name: dump_vgmdb_album_type_type_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_album_type_type_seq
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_album_type_type_seq OWNER TO ft;

--
-- Name: dump_vgmdb_album_type_type_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_album_type_type_seq OWNED BY public.dump_vgmdb_album_types.id;


--
-- Name: dump_vgmdb_album_websites; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_album_websites (
    albumid integer NOT NULL,
    catalog character varying(12) NOT NULL,
    name character varying(256) NOT NULL,
    link character varying(1024) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_album_websites OWNER TO ft;

--
-- Name: dump_vgmdb_albums; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_albums (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false NOT NULL,
    catalog character varying(32),
    release_date date,
    typeid smallint,
    classificationid integer,
    mediaformatid integer,
    meta_added_date timestamp without time zone,
    meta_edited_date timestamp without time zone,
    meta_fetched_date timestamp without time zone,
    meta_freedb integer,
    meta_ttl integer,
    meta_visitors integer,
    name character varying(255),
    notes character varying(65535),
    picture_full bytea,
    publishformatid integer,
    publisherid integer,
    rating double precision,
    release_currency character varying(4),
    release_price double precision,
    votes integer,
    scalerid integer
);


ALTER TABLE public.dump_vgmdb_albums OWNER TO ft;

--
-- Name: dump_vgmdb_artist; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_artist (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false,
    namelang character varying(7),
    name character varying(255),
    namereal character varying(192),
    birthdate date,
    birthplace character varying(96),
    meta_added_date timestamp without time zone,
    meta_edited_date timestamp without time zone,
    meta_fetched_date timestamp without time zone,
    meta_ttl integer,
    meta_visitors integer,
    notes character varying(4096),
    isfemale boolean,
    typeid integer,
    picture_full bytea,
    scalerid integer
);


ALTER TABLE public.dump_vgmdb_artist OWNER TO ft;

--
-- Name: dump_vgmdb_artist_alias; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_artist_alias (
    artistid integer NOT NULL,
    ordinal integer NOT NULL,
    lang character varying(8) NOT NULL,
    name character varying(255) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_artist_alias OWNER TO ft;

--
-- Name: dump_vgmdb_artist_featured; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_artist_featured (
    artistid integer NOT NULL,
    albumid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_artist_featured OWNER TO ft;

--
-- Name: dump_vgmdb_artist_type; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_artist_type (
    id integer NOT NULL,
    name character varying(16) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_artist_type OWNER TO ft;

--
-- Name: dump_vgmdb_artist_type_int_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_artist_type_int_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_artist_type_int_seq OWNER TO ft;

--
-- Name: dump_vgmdb_artist_type_int_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_artist_type_int_seq OWNED BY public.dump_vgmdb_artist_type.id;


--
-- Name: dump_vgmdb_artist_websites; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_artist_websites (
    artistid integer NOT NULL,
    catalog character varying(12) NOT NULL,
    name character varying(192) NOT NULL,
    link character varying(255) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_artist_websites OWNER TO ft;

--
-- Name: dump_vgmdb_event_releases; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_event_releases (
    eventid integer NOT NULL,
    albumid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_event_releases OWNER TO ft;

--
-- Name: dump_vgmdb_events; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_events (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false NOT NULL,
    year integer,
    enddate date,
    shortname character varying(64),
    startdate date,
    name character varying(255),
    notes character varying(1024)
);


ALTER TABLE public.dump_vgmdb_events OWNER TO ft;

--
-- Name: dump_vgmdb_events_translation; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_events_translation (
    id integer NOT NULL,
    lang character varying(7) NOT NULL,
    name character varying(255) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_events_translation OWNER TO ft;

--
-- Name: dump_vgmdb_label_regions; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_label_regions (
    id integer NOT NULL,
    name character varying(48) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_label_regions OWNER TO ft;

--
-- Name: dump_vgmdb_label_regions_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_label_regions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_label_regions_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_label_regions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_label_regions_id_seq OWNED BY public.dump_vgmdb_label_regions.id;


--
-- Name: dump_vgmdb_label_releases; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_label_releases (
    labelid integer NOT NULL,
    albumid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_label_releases OWNER TO ft;

--
-- Name: dump_vgmdb_label_staff; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_label_staff (
    labelid integer NOT NULL,
    artistid integer NOT NULL,
    owner boolean NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_label_staff OWNER TO ft;

--
-- Name: dump_vgmdb_label_types; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_label_types (
    id integer NOT NULL,
    name character varying(32) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_label_types OWNER TO ft;

--
-- Name: dump_vgmdb_label_types_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_label_types_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_label_types_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_label_types_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_label_types_id_seq OWNED BY public.dump_vgmdb_label_types.id;


--
-- Name: dump_vgmdb_label_websites; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_label_websites (
    labelid integer NOT NULL,
    catalog character varying(12) NOT NULL,
    name character varying(192) NOT NULL,
    link character varying(255) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_label_websites OWNER TO ft;

--
-- Name: dump_vgmdb_labels; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_labels (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false NOT NULL,
    name character varying(255),
    imprintid integer,
    formerlyid integer,
    subsidid integer,
    description character varying(1024),
    meta_added_date timestamp without time zone,
    meta_edited_date timestamp without time zone,
    meta_fetched_date timestamp without time zone,
    meta_ttl integer,
    meta_visitors integer,
    regionid integer,
    type integer
);


ALTER TABLE public.dump_vgmdb_labels OWNER TO ft;

--
-- Name: dump_vgmdb_product_albums; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_albums (
    productid integer NOT NULL,
    albumid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_albums OWNER TO ft;

--
-- Name: dump_vgmdb_product_labels; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_labels (
    productid integer NOT NULL,
    labelid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    arbitary boolean DEFAULT false NOT NULL,
    arbitaryname character varying(256)
);


ALTER TABLE public.dump_vgmdb_product_labels OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_albums; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_release_albums (
    releaseid integer NOT NULL,
    albumid integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_release_albums OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_arbitaries; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_release_arbitaries (
    productid integer NOT NULL,
    arrayindex integer NOT NULL,
    key character varying(12) NOT NULL,
    value character varying(255),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_release_arbitaries OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_platforms; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_release_platforms (
    id integer NOT NULL,
    name character varying(32) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_release_platforms OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_platforms_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_product_release_platforms_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_product_release_platforms_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_platforms_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_product_release_platforms_id_seq OWNED BY public.dump_vgmdb_product_release_platforms.id;


--
-- Name: dump_vgmdb_product_release_regions; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_release_regions (
    id integer NOT NULL,
    name character varying(64) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_release_regions OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_regions_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_product_release_regions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_product_release_regions_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_regions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_product_release_regions_id_seq OWNED BY public.dump_vgmdb_product_release_regions.id;


--
-- Name: dump_vgmdb_product_release_translations; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_release_translations (
    id integer NOT NULL,
    lang character varying(7) NOT NULL,
    name character varying(255) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_release_translations OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_types; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_release_types (
    id integer NOT NULL,
    name character varying(32) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_release_types OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_types_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_product_release_types_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_product_release_types_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_product_release_types_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_product_release_types_id_seq OWNED BY public.dump_vgmdb_product_release_types.id;


--
-- Name: dump_vgmdb_product_releases; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_releases (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false NOT NULL,
    platformid integer NOT NULL,
    regionid integer NOT NULL,
    catalog character varying(24),
    meta_added_date timestamp without time zone,
    meta_added_user character varying(32),
    meta_edited_date timestamp without time zone,
    meta_edited_user character varying(32),
    meta_fetched_date timestamp without time zone,
    meta_ttl integer,
    meta_visitors integer,
    name character varying(255),
    release_date date,
    release_type integer,
    type integer,
    upc character varying(28)
);


ALTER TABLE public.dump_vgmdb_product_releases OWNER TO ft;

--
-- Name: dump_vgmdb_product_types; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_types (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(64) NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_types OWNER TO ft;

--
-- Name: dump_vgmdb_product_types_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vgmdb_product_types_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vgmdb_product_types_id_seq OWNER TO ft;

--
-- Name: dump_vgmdb_product_types_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vgmdb_product_types_id_seq OWNED BY public.dump_vgmdb_product_types.id;


--
-- Name: dump_vgmdb_product_websites; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_product_websites (
    productid integer NOT NULL,
    catalog character varying(16) NOT NULL,
    name character varying(384) NOT NULL,
    link character varying(512) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vgmdb_product_websites OWNER TO ft;

--
-- Name: dump_vgmdb_products; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vgmdb_products (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false,
    name character varying(255) NOT NULL,
    typeid integer,
    description character varying(1024),
    meta_added_date timestamp without time zone,
    meta_added_user character varying(64),
    meta_edited_date timestamp without time zone,
    meta_edited_user character varying(64),
    meta_fetched_date timestamp without time zone,
    meta_ttl integer,
    meta_visitors integer,
    name_real character varying(255),
    picture_full bytea,
    release_date date,
    parent_franchise integer,
    scalerid integer
);


ALTER TABLE public.dump_vgmdb_products OWNER TO ft;

--
-- Name: dump_vndb_0dumpmeta_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vndb_0dumpmeta_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vndb_0dumpmeta_id_seq OWNER TO ft;

--
-- Name: dump_vndb_0dumpmeta_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vndb_0dumpmeta_id_seq OWNED BY public.dump_vgmdb_0dumpmeta.id;


--
-- Name: dump_vndb_character; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_character (
    id integer NOT NULL,
    name character varying(128) NOT NULL,
    original character varying(128),
    gender character(1),
    bloodt character varying(2),
    birthday character varying(6),
    aliases character varying(256),
    description character varying(4096),
    image bytea,
    bust integer,
    waist integer,
    hip integer,
    height integer,
    weight integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scalerid integer
);


ALTER TABLE public.dump_vndb_character OWNER TO ft;

--
-- Name: dump_vndb_character_instances; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_character_instances (
    cid integer NOT NULL,
    id integer NOT NULL,
    spoiler integer,
    name character varying(128),
    original character varying(192),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_character_instances OWNER TO ft;

--
-- Name: dump_vndb_character_traits; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_character_traits (
    cid integer NOT NULL,
    tid integer NOT NULL,
    spoilerlevel integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_character_traits OWNER TO ft;

--
-- Name: dump_vndb_character_vns; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_character_vns (
    cid integer NOT NULL,
    vnid integer NOT NULL,
    rid integer,
    spoilerlevel integer,
    role character varying(7),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_character_vns OWNER TO ft;

--
-- Name: dump_vndb_character_voiced; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_character_voiced (
    cid integer NOT NULL,
    id integer NOT NULL,
    aid integer,
    vid integer,
    note character varying(1024),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    relationid bigint NOT NULL
);


ALTER TABLE public.dump_vndb_character_voiced OWNER TO ft;

--
-- Name: dump_vndb_character_voiced_relationid_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vndb_character_voiced_relationid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vndb_character_voiced_relationid_seq OWNER TO ft;

--
-- Name: dump_vndb_character_voiced_relationid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vndb_character_voiced_relationid_seq OWNED BY public.dump_vndb_character_voiced.relationid;


--
-- Name: dump_vndb_release; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_release (
    id integer NOT NULL,
    title character varying(256) NOT NULL,
    original character varying(128),
    released date,
    type character varying(8) NOT NULL,
    patch boolean DEFAULT false NOT NULL,
    freeware boolean DEFAULT false NOT NULL,
    doujin boolean DEFAULT false NOT NULL,
    website character varying(384),
    notes character varying(2048),
    minage integer,
    gtin character varying(13),
    catalog character varying(24),
    resolution character varying(12),
    voiced integer,
    animation_story integer,
    animation_ero integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_release OWNER TO ft;

--
-- Name: dump_vndb_release_languages; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_release_languages (
    rid integer NOT NULL,
    lang character varying(5) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_release_languages OWNER TO ft;

--
-- Name: dump_vndb_release_media; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_release_media (
    rid integer NOT NULL,
    medium character varying(8) NOT NULL,
    qty integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_release_media OWNER TO ft;

--
-- Name: dump_vndb_release_platforms; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_release_platforms (
    rid integer NOT NULL,
    platform character varying(8) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_release_platforms OWNER TO ft;

--
-- Name: dump_vndb_release_producers; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_release_producers (
    rid integer NOT NULL,
    pid integer NOT NULL,
    developer boolean,
    publisher boolean,
    name character varying(128),
    original character varying(128),
    type character varying(32),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_release_producers OWNER TO ft;

--
-- Name: dump_vndb_release_vns; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_release_vns (
    rid integer NOT NULL,
    vnid integer NOT NULL,
    title character varying(256),
    original character varying(128),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_release_vns OWNER TO ft;

--
-- Name: dump_vndb_tags; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_tags (
    id integer NOT NULL,
    name character varying(128) NOT NULL,
    description character varying(2048) NOT NULL,
    meta boolean NOT NULL,
    vns integer NOT NULL,
    cat character varying(4) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.dump_vndb_tags OWNER TO ft;

--
-- Name: dump_vndb_tags_aliases; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_tags_aliases (
    tagid integer NOT NULL,
    name character varying(128) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_tags_aliases OWNER TO ft;

--
-- Name: dump_vndb_tags_parents; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_tags_parents (
    child integer NOT NULL,
    parent integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_tags_parents OWNER TO ft;

--
-- Name: dump_vndb_traits; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_traits (
    id integer NOT NULL,
    name character varying(128) NOT NULL,
    description character varying(2048) NOT NULL,
    meta boolean NOT NULL,
    chars integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.dump_vndb_traits OWNER TO ft;

--
-- Name: dump_vndb_traits_aliases; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_traits_aliases (
    id integer NOT NULL,
    alias character varying(128) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_traits_aliases OWNER TO ft;

--
-- Name: dump_vndb_traits_parents; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_traits_parents (
    child integer NOT NULL,
    parent integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_traits_parents OWNER TO ft;

--
-- Name: dump_vndb_vn; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scraped boolean DEFAULT false NOT NULL,
    title character varying(256),
    original character varying(192),
    released date,
    aliases character varying(1024),
    length integer,
    description character varying(4096),
    wikipedia character varying(255),
    encubed character varying(255),
    renai character varying(255),
    image bytea,
    image_nsfw boolean DEFAULT false NOT NULL,
    popularity double precision,
    rating double precision,
    votecount integer,
    scalerid integer
);


ALTER TABLE public.dump_vndb_vn OWNER TO ft;

--
-- Name: dump_vndb_vn_anime; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn_anime (
    relationid integer NOT NULL,
    vnid integer,
    id integer,
    ann_id integer,
    nfo_id character varying(64),
    title_romaji character varying(256),
    title_kanji character varying(192),
    year integer,
    type character varying(16),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_vn_anime OWNER TO ft;

--
-- Name: dump_vndb_vn_anime_relationid_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vndb_vn_anime_relationid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vndb_vn_anime_relationid_seq OWNER TO ft;

--
-- Name: dump_vndb_vn_anime_relationid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vndb_vn_anime_relationid_seq OWNED BY public.dump_vndb_vn_anime.relationid;


--
-- Name: dump_vndb_vn_languages; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn_languages (
    vnid integer NOT NULL,
    language character varying(5) NOT NULL,
    orig_lang boolean NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_vn_languages OWNER TO ft;

--
-- Name: dump_vndb_vn_platforms; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn_platforms (
    vnid integer NOT NULL,
    platform character varying(16) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.dump_vndb_vn_platforms OWNER TO ft;

--
-- Name: dump_vndb_vn_relation; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn_relation (
    srcid integer NOT NULL,
    id integer NOT NULL,
    relation character varying(16),
    title character varying(256),
    original character varying(192),
    official boolean,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_vn_relation OWNER TO ft;

--
-- Name: dump_vndb_vn_screens; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn_screens (
    screenid integer NOT NULL,
    imageurl character varying(256),
    image bytea,
    rid integer,
    nsfw boolean,
    height integer,
    width integer,
    vnid integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    scalerid integer
);


ALTER TABLE public.dump_vndb_vn_screens OWNER TO ft;

--
-- Name: dump_vndb_vn_screens_screenid_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vndb_vn_screens_screenid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vndb_vn_screens_screenid_seq OWNER TO ft;

--
-- Name: dump_vndb_vn_screens_screenid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vndb_vn_screens_screenid_seq OWNED BY public.dump_vndb_vn_screens.screenid;


--
-- Name: dump_vndb_vn_staff; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn_staff (
    id bigint NOT NULL,
    vnid integer,
    sid integer,
    aid integer,
    name character varying(64),
    original character varying(64),
    role character varying(64),
    note character varying(256),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_vn_staff OWNER TO ft;

--
-- Name: dump_vndb_vn_staff_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vndb_vn_staff_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vndb_vn_staff_id_seq OWNER TO ft;

--
-- Name: dump_vndb_vn_staff_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vndb_vn_staff_id_seq OWNED BY public.dump_vndb_vn_staff.id;


--
-- Name: dump_vndb_vn_tag; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_vn_tag (
    srcid integer NOT NULL,
    tagid integer NOT NULL,
    score double precision,
    spoilerlevel integer,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.dump_vndb_vn_tag OWNER TO ft;

--
-- Name: dump_vndb_votes; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vndb_votes (
    vnid integer NOT NULL,
    userid integer NOT NULL,
    vote smallint NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    dateutime integer NOT NULL
);


ALTER TABLE public.dump_vndb_votes OWNER TO ft;

--
-- Name: dump_vocadb_0banevasion; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vocadb_0banevasion (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    requests integer NOT NULL
);


ALTER TABLE public.dump_vocadb_0banevasion OWNER TO ft;

--
-- Name: dump_vocadb_0dumpmeta; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vocadb_0dumpmeta (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    key1 character varying(128) NOT NULL,
    key2 character varying(96),
    keyutime integer
);


ALTER TABLE public.dump_vocadb_0dumpmeta OWNER TO ft;

--
-- Name: dump_vocadb_0dumpmeta_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.dump_vocadb_0dumpmeta_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.dump_vocadb_0dumpmeta_id_seq OWNER TO ft;

--
-- Name: dump_vocadb_0dumpmeta_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.dump_vocadb_0dumpmeta_id_seq OWNED BY public.dump_vocadb_0dumpmeta.id;


--
-- Name: dump_vocadb_albums; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vocadb_albums (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(256),
    artiststring character varying(128),
    createdate timestamp without time zone,
    defaultnamelanguage character varying(64),
    disctype character varying(48),
    ratingaverage double precision,
    ratingcount integer,
    releasedate date,
    status character varying(32),
    version integer,
    scrapedtracks boolean DEFAULT false NOT NULL,
    defaultname character varying(255),
    cover bytea,
    catalognumber character varying(56),
    scalerid integer,
    scrapedcover boolean DEFAULT false NOT NULL
);


ALTER TABLE public.dump_vocadb_albums OWNER TO ft;

--
-- Name: dump_vocadb_albumtracks; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vocadb_albumtracks (
    id bigint NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(255),
    discnumber integer,
    songid bigint,
    tracknumber integer,
    albumid integer NOT NULL
);


ALTER TABLE public.dump_vocadb_albumtracks OWNER TO ft;

--
-- Name: dump_vocadb_artist; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vocadb_artist (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    additionalnames text,
    artisttype character varying(48),
    deleted boolean,
    name character varying(256),
    picturemime character varying(64),
    status character varying(32),
    version integer,
    scraped boolean DEFAULT false NOT NULL,
    image bytea,
    scalerid integer
);


ALTER TABLE public.dump_vocadb_artist OWNER TO ft;

--
-- Name: dump_vocadb_songartists; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vocadb_songartists (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    trackid integer NOT NULL,
    artistid integer,
    categories character varying(128),
    effectiveroles character varying(192),
    iscustomname boolean,
    issupport boolean,
    name character varying(255),
    roles character varying(172)
);


ALTER TABLE public.dump_vocadb_songartists OWNER TO ft;

--
-- Name: dump_vocadb_songs; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.dump_vocadb_songs (
    id bigint NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    artiststring character varying(128),
    createdate timestamp without time zone,
    defaultname character varying(256),
    defaultnamelanguage character varying(16),
    favoritedtimes integer,
    lengthseconds integer,
    name character varying(384),
    publishdate timestamp without time zone,
    pvservices character varying(192),
    ratingscore double precision,
    songtype character varying(32),
    status character varying(48),
    version integer
);


ALTER TABLE public.dump_vocadb_songs OWNER TO ft;

--
-- Name: hibernate_sequences; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.hibernate_sequences (
    sequence_name character varying(255),
    next_val integer
);


ALTER TABLE public.hibernate_sequences OWNER TO ft;

--
-- Name: licensing_fatclient_machines; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.licensing_fatclient_machines (
    uid character varying(47) NOT NULL,
    machinename character varying(64) NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    state integer NOT NULL,
    osversion text,
    amd64cpu boolean,
    amd64task boolean,
    cpus integer,
    systemdir character varying(255),
    pagesize integer,
    userdomainname character varying(128),
    username character varying(64),
    clrversion character varying(20),
    path text,
    debuggee boolean,
    dateupdated timestamp without time zone,
    remark text
);


ALTER TABLE public.licensing_fatclient_machines OWNER TO ft;

--
-- Name: mailarchive_folders; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.mailarchive_folders (
    id bigint NOT NULL,
    name character varying(255) NOT NULL,
    parent bigint,
    dateadded timestamp without time zone
);


ALTER TABLE public.mailarchive_folders OWNER TO ft;

--
-- Name: mailarchive_mails; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.mailarchive_mails (
    uid bigint NOT NULL,
    "messageUtime" integer NOT NULL,
    folder bigint NOT NULL,
    dateadded timestamp without time zone NOT NULL,
    sender character varying(250),
    recipient character varying(250),
    subject character varying(500),
    salt bytea,
    iterations smallint,
    data bytea
);


ALTER TABLE public.mailarchive_mails OWNER TO ft;

--
-- Name: notebook_notes; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.notebook_notes (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    iscategory boolean NOT NULL,
    richtext text,
    dateupdated timestamp without time zone,
    parent integer,
    name character varying(255) NOT NULL
);


ALTER TABLE public.notebook_notes OWNER TO ft;

--
-- Name: notebook_notes_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.notebook_notes_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.notebook_notes_id_seq OWNER TO ft;

--
-- Name: notebook_notes_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.notebook_notes_id_seq OWNED BY public.notebook_notes.id;


--
-- Name: osm_node_tags; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_node_tags (
    "nodeId" bigint NOT NULL,
    k character varying(128) NOT NULL,
    v character varying(128),
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_node_tags OWNER TO ft;

--
-- Name: osm_nodes; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_nodes (
    id bigint NOT NULL,
    lat double precision NOT NULL,
    lon double precision NOT NULL,
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_nodes OWNER TO ft;

--
-- Name: osm_relation_member; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_relation_member (
    "relationId" bigint NOT NULL,
    type character varying(10) NOT NULL,
    reference bigint NOT NULL,
    role character varying(10),
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_relation_member OWNER TO ft;

--
-- Name: osm_relation_tags; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_relation_tags (
    "relationId" bigint NOT NULL,
    k character varying(128) NOT NULL,
    v character varying(128),
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_relation_tags OWNER TO ft;

--
-- Name: osm_relations; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_relations (
    id bigint NOT NULL,
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_relations OWNER TO ft;

--
-- Name: osm_way_nodes; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_way_nodes (
    way bigint NOT NULL,
    node bigint NOT NULL,
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_way_nodes OWNER TO ft;

--
-- Name: osm_way_tags; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_way_tags (
    "wayId" bigint NOT NULL,
    k character varying(128) NOT NULL,
    v character varying(128),
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_way_tags OWNER TO ft;

--
-- Name: osm_ways; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.osm_ways (
    id bigint NOT NULL,
    "dateAdded" timestamp without time zone NOT NULL
);


ALTER TABLE public.osm_ways OWNER TO ft;

--
-- Name: patchouli_backupsets; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.patchouli_backupsets (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    name character varying(255) NOT NULL,
    machineid integer NOT NULL,
    firsttapelength integer NOT NULL,
    rootdir character varying(255) NOT NULL
);


ALTER TABLE public.patchouli_backupsets OWNER TO ft;

--
-- Name: patchouli_backupsets_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.patchouli_backupsets_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.patchouli_backupsets_id_seq OWNER TO ft;

--
-- Name: patchouli_backupsets_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.patchouli_backupsets_id_seq OWNED BY public.patchouli_backupsets.id;


--
-- Name: patchouli_filecatalog; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.patchouli_filecatalog (
    id bigint NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    tapeid integer NOT NULL,
    filename character varying(384) NOT NULL,
    size bigint NOT NULL,
    ctime timestamp without time zone,
    mtime timestamp without time zone,
    qhash character varying(40),
    fauxhash bigint,
    backupset integer NOT NULL,
    dateupdated timestamp without time zone
);


ALTER TABLE public.patchouli_filecatalog OWNER TO ft;

--
-- Name: patchouli_filecatalog_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.patchouli_filecatalog_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.patchouli_filecatalog_id_seq OWNER TO ft;

--
-- Name: patchouli_filecatalog_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.patchouli_filecatalog_id_seq OWNED BY public.patchouli_filecatalog.id;


--
-- Name: patchouli_machines; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.patchouli_machines (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    ip inet DEFAULT inet_client_addr(),
    hostname character varying(64) NOT NULL,
    mac character varying(17) NOT NULL
);


ALTER TABLE public.patchouli_machines OWNER TO ft;

--
-- Name: patchouli_machines_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.patchouli_machines_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.patchouli_machines_id_seq OWNER TO ft;

--
-- Name: patchouli_machines_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.patchouli_machines_id_seq OWNED BY public.patchouli_machines.id;


--
-- Name: patchouli_smart_disks; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.patchouli_smart_disks (
    id integer NOT NULL,
    model character varying(64) NOT NULL,
    serialno character varying(32) NOT NULL,
    wwn character varying(32) NOT NULL,
    firmware character varying(32),
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    firstmachine integer NOT NULL,
    decommissioned date,
    capacity bigint
);


ALTER TABLE public.patchouli_smart_disks OWNER TO ft;

--
-- Name: patchouli_smart_disks_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.patchouli_smart_disks_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.patchouli_smart_disks_id_seq OWNER TO ft;

--
-- Name: patchouli_smart_disks_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.patchouli_smart_disks_id_seq OWNED BY public.patchouli_smart_disks.id;


--
-- Name: patchouli_smart_values; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.patchouli_smart_values (
    dateadded timestamp without time zone NOT NULL,
    driveid integer NOT NULL,
    "0" integer,
    "1" integer,
    "2" integer,
    "3" integer,
    "4" integer,
    "5" integer,
    "6" integer,
    "7" bigint,
    "8" integer,
    "9" integer,
    "10" integer,
    "11" integer,
    "12" integer,
    "13" integer,
    "14" integer,
    "15" integer,
    "16" integer,
    "17" integer,
    "18" integer,
    "19" integer,
    "20" integer,
    "21" integer,
    "22" integer,
    "23" integer,
    "24" integer,
    "25" integer,
    "26" integer,
    "27" integer,
    "28" integer,
    "29" integer,
    "30" integer,
    "31" integer,
    "32" integer,
    "33" integer,
    "34" integer,
    "35" integer,
    "36" integer,
    "37" integer,
    "38" integer,
    "39" integer,
    "40" integer,
    "41" integer,
    "42" integer,
    "43" integer,
    "44" integer,
    "45" integer,
    "46" integer,
    "47" integer,
    "48" integer,
    "49" integer,
    "50" integer,
    "51" integer,
    "52" integer,
    "53" integer,
    "54" integer,
    "55" integer,
    "56" integer,
    "57" integer,
    "58" integer,
    "59" integer,
    "60" integer,
    "61" integer,
    "62" integer,
    "63" integer,
    "64" integer,
    "65" integer,
    "66" integer,
    "67" integer,
    "68" integer,
    "69" integer,
    "70" integer,
    "71" integer,
    "72" integer,
    "73" integer,
    "74" integer,
    "75" integer,
    "76" integer,
    "77" integer,
    "78" integer,
    "79" integer,
    "80" integer,
    "81" integer,
    "82" integer,
    "83" integer,
    "84" integer,
    "85" integer,
    "86" integer,
    "87" integer,
    "88" integer,
    "89" integer,
    "90" integer,
    "91" integer,
    "92" integer,
    "93" integer,
    "94" integer,
    "95" integer,
    "96" integer,
    "97" integer,
    "98" integer,
    "99" integer,
    "100" integer,
    "101" integer,
    "102" integer,
    "103" integer,
    "104" integer,
    "105" integer,
    "106" integer,
    "107" integer,
    "108" integer,
    "109" integer,
    "110" integer,
    "111" integer,
    "112" integer,
    "113" integer,
    "114" integer,
    "115" integer,
    "116" integer,
    "117" integer,
    "118" integer,
    "119" integer,
    "120" integer,
    "121" integer,
    "122" integer,
    "123" integer,
    "124" integer,
    "125" integer,
    "126" integer,
    "127" integer,
    "128" integer,
    "129" integer,
    "130" integer,
    "131" integer,
    "132" integer,
    "133" integer,
    "134" integer,
    "135" integer,
    "136" integer,
    "137" integer,
    "138" integer,
    "139" integer,
    "140" integer,
    "141" integer,
    "142" integer,
    "143" integer,
    "144" integer,
    "145" integer,
    "146" integer,
    "147" integer,
    "148" integer,
    "149" integer,
    "150" integer,
    "151" integer,
    "152" integer,
    "153" integer,
    "154" integer,
    "155" integer,
    "156" integer,
    "157" integer,
    "158" integer,
    "159" integer,
    "160" integer,
    "161" integer,
    "162" integer,
    "163" integer,
    "164" integer,
    "165" integer,
    "166" integer,
    "167" integer,
    "168" integer,
    "169" integer,
    "170" integer,
    "171" integer,
    "172" integer,
    "173" integer,
    "174" integer,
    "175" integer,
    "176" integer,
    "177" integer,
    "178" integer,
    "179" integer,
    "180" integer,
    "181" integer,
    "182" integer,
    "183" integer,
    "184" integer,
    "185" integer,
    "186" integer,
    "187" bigint,
    "188" bigint,
    "189" integer,
    "190" integer,
    "191" integer,
    "192" integer,
    "193" integer,
    "194" integer,
    "195" integer,
    "196" integer,
    "197" integer,
    "198" integer,
    "199" integer,
    "200" integer,
    "201" integer,
    "202" integer,
    "203" integer,
    "204" integer,
    "205" integer,
    "206" integer,
    "207" integer,
    "208" integer,
    "209" integer,
    "210" integer,
    "211" integer,
    "212" integer,
    "213" integer,
    "214" integer,
    "215" integer,
    "216" integer,
    "217" integer,
    "218" integer,
    "219" integer,
    "220" integer,
    "221" integer,
    "222" integer,
    "223" integer,
    "224" integer,
    "225" integer,
    "226" integer,
    "227" integer,
    "228" integer,
    "229" integer,
    "230" integer,
    "231" integer,
    "232" integer,
    "233" integer,
    "234" integer,
    "235" integer,
    "236" integer,
    "237" integer,
    "238" integer,
    "239" integer,
    "240" integer,
    "241" bigint,
    "242" bigint,
    "243" integer,
    "244" integer,
    "245" integer,
    "246" integer,
    "247" integer,
    "248" integer,
    "249" integer,
    "250" integer,
    "251" integer,
    "252" integer,
    "253" integer,
    "254" integer,
    "255" integer
);


ALTER TABLE public.patchouli_smart_values OWNER TO ft;

--
-- Name: patchouli_tapes; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.patchouli_tapes (
    id integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    gigabytes integer NOT NULL
);


ALTER TABLE public.patchouli_tapes OWNER TO ft;

--
-- Name: patchouli_tapes_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.patchouli_tapes_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.patchouli_tapes_id_seq OWNER TO ft;

--
-- Name: patchouli_tapes_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.patchouli_tapes_id_seq OWNED BY public.patchouli_tapes.id;


--
-- Name: sedgetree_photos; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.sedgetree_photos (
    person character varying(39) NOT NULL,
    "dateAdded" timestamp without time zone NOT NULL,
    "photoData" bytea
);


ALTER TABLE public.sedgetree_photos OWNER TO ft;

--
-- Name: sedgetree_versioning; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.sedgetree_versioning (
    id integer NOT NULL,
    dateadded timestamp without time zone NOT NULL,
    data bytea NOT NULL
);


ALTER TABLE public.sedgetree_versioning OWNER TO ft;

--
-- Name: tarbuddy_schedule; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.tarbuddy_schedule (
    date date NOT NULL,
    event character varying(100) NOT NULL,
    dateadded timestamp without time zone NOT NULL
);


ALTER TABLE public.tarbuddy_schedule OWNER TO ft;

--
-- Name: warwalking_discoveries; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.warwalking_discoveries (
    bssid character varying(17) NOT NULL,
    ssid character varying(64) NOT NULL,
    rssi integer NOT NULL,
    lon double precision NOT NULL,
    lat double precision NOT NULL,
    "dot11BssType" integer,
    "dot11DefaultAuthAlgorithm" integer,
    "dot11DefaultCipherAlgorithm" integer,
    "discoveryUtime" integer,
    flags integer,
    "morePhyTypes" smallint,
    "networkConnectable" smallint,
    "securityEnabled" smallint,
    "wlanSignalQuality" integer,
    "beaconPeriod" integer,
    "capabilityInformation" integer,
    "chCenterFrequency" integer,
    "dot11BssPhyType" integer,
    "hostTimestamp" bigint,
    "ieOffset" integer,
    "ieSize" integer,
    "inRegDomain" smallint,
    "phyId" integer,
    "timestamp" bigint,
    mbps integer,
    dateadded timestamp without time zone NOT NULL,
    "relatedTour" integer NOT NULL,
    "strongestSignalUtime" integer NOT NULL,
    "strongestTour" integer
);


ALTER TABLE public.warwalking_discoveries OWNER TO ft;

--
-- Name: warwalking_tours; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.warwalking_tours (
    id integer NOT NULL,
    hash bigint NOT NULL,
    "utimeRecordingStarted" integer NOT NULL,
    name character varying(128) NOT NULL,
    dateadded timestamp without time zone NOT NULL
);


ALTER TABLE public.warwalking_tours OWNER TO ft;

--
-- Name: web_beacon; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.web_beacon (
    id integer NOT NULL,
    dateadded timestamp without time zone NOT NULL,
    dateupdated timestamp without time zone NOT NULL,
    name character varying(255) NOT NULL,
    remoteaddr character varying(128),
    host character varying(128),
    useragent text,
    accept character varying(128)
);


ALTER TABLE public.web_beacon OWNER TO ft;

--
-- Name: web_beacon_id_seq; Type: SEQUENCE; Schema: public; Owner: ft
--

CREATE SEQUENCE public.web_beacon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.web_beacon_id_seq OWNER TO ft;

--
-- Name: web_beacon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ft
--

ALTER SEQUENCE public.web_beacon_id_seq OWNED BY public.web_beacon.id;


--
-- Name: web_captiveportal; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.web_captiveportal (
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    dnsname text,
    requestpath text,
    clientip character varying(16),
    clientmac character varying(18),
    useragent text,
    randomvalue integer NOT NULL
);


ALTER TABLE public.web_captiveportal OWNER TO ft;

--
-- Name: web_heartbeat; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.web_heartbeat (
    id timestamp without time zone NOT NULL,
    rand integer NOT NULL,
    dateadded timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.web_heartbeat OWNER TO postgres;

--
-- Name: web_startup; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.web_startup (
    id integer NOT NULL,
    dateadded timestamp without time zone NOT NULL,
    machinename character varying(64) NOT NULL
);


ALTER TABLE public.web_startup OWNER TO postgres;

--
-- Name: web_startup_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.web_startup_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.web_startup_id_seq OWNER TO postgres;

--
-- Name: web_startup_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.web_startup_id_seq OWNED BY public.web_startup.id;


--
-- Name: web_users; Type: TABLE; Schema: public; Owner: ft
--

CREATE TABLE public.web_users (
    id integer NOT NULL,
    dateadded timestamp without time zone NOT NULL,
    username character varying(64) NOT NULL,
    displayname character varying(128),
    profilepic bytea,
    password character varying(64)
);


ALTER TABLE public.web_users OWNER TO ft;

--
-- Name: azusa_countries id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_countries ALTER COLUMN id SET DEFAULT nextval('public.azusa_countries_id_seq'::regclass);


--
-- Name: azusa_filesysteminfo id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_filesysteminfo ALTER COLUMN id SET DEFAULT nextval('public.azusa_filesysteminfo_id_seq'::regclass);


--
-- Name: azusa_languages id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_languages ALTER COLUMN id SET DEFAULT nextval('public.azusa_languages_id_seq'::regclass);


--
-- Name: azusa_media id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_media ALTER COLUMN id SET DEFAULT nextval('public.azusa_media_id_seq'::regclass);


--
-- Name: azusa_platforms id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_platforms ALTER COLUMN id SET DEFAULT nextval('public.azusa_platforms_id_seq'::regclass);


--
-- Name: azusa_products id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_products ALTER COLUMN id SET DEFAULT nextval('public.azusa_products_id_seq'::regclass);


--
-- Name: cddb_category id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cddb_category ALTER COLUMN id SET DEFAULT nextval('public.cddb_category_id_seq'::regclass);


--
-- Name: cddb_disc sqlid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cddb_disc ALTER COLUMN sqlid SET DEFAULT nextval('public.cddb_disc_sqlid_seq'::regclass);


--
-- Name: discarchivator_joblist id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.discarchivator_joblist ALTER COLUMN id SET DEFAULT nextval('public.discarchivator_joblist_id_seq'::regclass);


--
-- Name: discarchivator_tasklog id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.discarchivator_tasklog ALTER COLUMN id SET DEFAULT nextval('public.discarchivator_tasklog_id_seq'::regclass);


--
-- Name: dump_gb_tags id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_gb_tags ALTER COLUMN id SET DEFAULT nextval('public.dump_gb_tags_seq_seq'::regclass);


--
-- Name: dump_myfigurecollection_0dumpmeta id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_0dumpmeta ALTER COLUMN id SET DEFAULT nextval('public.dump_myfigurecollection_0dumpmeta_id_seq'::regclass);


--
-- Name: dump_myfigurecollection_0statistics id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_0statistics ALTER COLUMN id SET DEFAULT nextval('public.dump_myfigurecollection_0statistics_id_seq'::regclass);


--
-- Name: dump_myfigurecollection_categories id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_categories ALTER COLUMN id SET DEFAULT nextval('public.dump_myfigurecollection_categories_id_seq'::regclass);


--
-- Name: dump_myfigurecollection_roots id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_roots ALTER COLUMN id SET DEFAULT nextval('public.dump_myfigurecollection_roots_id_seq'::regclass);


--
-- Name: dump_psxdatacenter_companies id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_companies ALTER COLUMN id SET DEFAULT nextval('public.dump_psxdatacenter_companies_id_seq'::regclass);


--
-- Name: dump_psxdatacenter_game_screenshots hibernateid; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_game_screenshots ALTER COLUMN hibernateid SET DEFAULT nextval('public.dump_psxdatacenter_game_screenshots_hibernateid_seq'::regclass);


--
-- Name: dump_psxdatacenter_games id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_games ALTER COLUMN id SET DEFAULT nextval('public.dump_psxdatacenter_games_id_seq'::regclass);


--
-- Name: dump_psxdatacenter_genres id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_genres ALTER COLUMN id SET DEFAULT nextval('public.dump_psxdatacenter_genres_id_seq'::regclass);


--
-- Name: dump_psxdatacenter_screenshots id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_screenshots ALTER COLUMN id SET DEFAULT nextval('public.dump_psxdatacenter_screenshotdata_id_seq'::regclass);


--
-- Name: dump_vgmdb_0dumpmeta id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_0dumpmeta ALTER COLUMN id SET DEFAULT nextval('public.dump_vndb_0dumpmeta_id_seq'::regclass);


--
-- Name: dump_vgmdb_0errors id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_0errors ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_0errors_id_seq'::regclass);


--
-- Name: dump_vgmdb_album_artist_type id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_artist_type ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_album_artist_type_id_seq'::regclass);


--
-- Name: dump_vgmdb_album_classification id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_classification ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_album_classification_id_seq'::regclass);


--
-- Name: dump_vgmdb_album_label_roles id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_label_roles ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_album_label_roles_id_seq'::regclass);


--
-- Name: dump_vgmdb_album_mediaformat id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_mediaformat ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_album_mediaformat_id_seq'::regclass);


--
-- Name: dump_vgmdb_album_types id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_types ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_album_type_type_seq'::regclass);


--
-- Name: dump_vgmdb_artist_type id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_artist_type ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_artist_type_int_seq'::regclass);


--
-- Name: dump_vgmdb_label_regions id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_label_regions ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_label_regions_id_seq'::regclass);


--
-- Name: dump_vgmdb_label_types id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_label_types ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_label_types_id_seq'::regclass);


--
-- Name: dump_vgmdb_product_release_platforms id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_platforms ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_product_release_platforms_id_seq'::regclass);


--
-- Name: dump_vgmdb_product_release_regions id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_regions ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_product_release_regions_id_seq'::regclass);


--
-- Name: dump_vgmdb_product_release_types id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_types ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_product_release_types_id_seq'::regclass);


--
-- Name: dump_vgmdb_product_types id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_types ALTER COLUMN id SET DEFAULT nextval('public.dump_vgmdb_product_types_id_seq'::regclass);


--
-- Name: dump_vndb_character_voiced relationid; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_character_voiced ALTER COLUMN relationid SET DEFAULT nextval('public.dump_vndb_character_voiced_relationid_seq'::regclass);


--
-- Name: dump_vndb_vn_anime relationid; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_anime ALTER COLUMN relationid SET DEFAULT nextval('public.dump_vndb_vn_anime_relationid_seq'::regclass);


--
-- Name: dump_vndb_vn_screens screenid; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_screens ALTER COLUMN screenid SET DEFAULT nextval('public.dump_vndb_vn_screens_screenid_seq'::regclass);


--
-- Name: dump_vndb_vn_staff id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_staff ALTER COLUMN id SET DEFAULT nextval('public.dump_vndb_vn_staff_id_seq'::regclass);


--
-- Name: dump_vocadb_0dumpmeta id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vocadb_0dumpmeta ALTER COLUMN id SET DEFAULT nextval('public.dump_vocadb_0dumpmeta_id_seq'::regclass);


--
-- Name: notebook_notes id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.notebook_notes ALTER COLUMN id SET DEFAULT nextval('public.notebook_notes_id_seq'::regclass);


--
-- Name: patchouli_backupsets id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_backupsets ALTER COLUMN id SET DEFAULT nextval('public.patchouli_backupsets_id_seq'::regclass);


--
-- Name: patchouli_filecatalog id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_filecatalog ALTER COLUMN id SET DEFAULT nextval('public.patchouli_filecatalog_id_seq'::regclass);


--
-- Name: patchouli_machines id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_machines ALTER COLUMN id SET DEFAULT nextval('public.patchouli_machines_id_seq'::regclass);


--
-- Name: patchouli_smart_disks id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_smart_disks ALTER COLUMN id SET DEFAULT nextval('public.patchouli_smart_disks_id_seq'::regclass);


--
-- Name: patchouli_tapes id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_tapes ALTER COLUMN id SET DEFAULT nextval('public.patchouli_tapes_id_seq'::regclass);


--
-- Name: web_beacon id; Type: DEFAULT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.web_beacon ALTER COLUMN id SET DEFAULT nextval('public.web_beacon_id_seq'::regclass);


--
-- Name: web_startup id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.web_startup ALTER COLUMN id SET DEFAULT nextval('public.web_startup_id_seq'::regclass);


--
-- Name: azusa_countries azusa_countries_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_countries
    ADD CONSTRAINT azusa_countries_pkey PRIMARY KEY (id);


--
-- Name: azusa_filesysteminfo azusa_filesysteminfo_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_filesysteminfo
    ADD CONSTRAINT azusa_filesysteminfo_pk PRIMARY KEY (id);


--
-- Name: azusa_languages azusa_languages_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_languages
    ADD CONSTRAINT azusa_languages_pkey PRIMARY KEY (id);


--
-- Name: azusa_mediatypes azusa_mediaTypes_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_mediatypes
    ADD CONSTRAINT "azusa_mediaTypes_pkey" PRIMARY KEY (id);


--
-- Name: azusa_media azusa_media_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_media
    ADD CONSTRAINT azusa_media_pkey PRIMARY KEY (id);


--
-- Name: azusa_platforms azusa_platforms_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_platforms
    ADD CONSTRAINT azusa_platforms_pkey PRIMARY KEY (id);


--
-- Name: azusa_products azusa_products_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_products
    ADD CONSTRAINT azusa_products_pkey PRIMARY KEY (id);


--
-- Name: azusa_shelves azusa_shelves_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_shelves
    ADD CONSTRAINT azusa_shelves_pkey PRIMARY KEY (id);


--
-- Name: azusa_shops azusa_shops_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_shops
    ADD CONSTRAINT azusa_shops_pkey PRIMARY KEY (id);


--
-- Name: azusa_statistics azusa_statistics_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.azusa_statistics
    ADD CONSTRAINT azusa_statistics_pkey PRIMARY KEY (date);


--
-- Name: cddb_category cddb_category_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cddb_category
    ADD CONSTRAINT cddb_category_pk PRIMARY KEY (id);


--
-- Name: cddb_disc cddb_disc_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cddb_disc
    ADD CONSTRAINT cddb_disc_pk PRIMARY KEY (category, discid);


--
-- Name: cddb_track cddb_track_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cddb_track
    ADD CONSTRAINT cddb_track_pk UNIQUE (discsqlid, trackno);


--
-- Name: dexcom_history dexcom_history_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dexcom_history
    ADD CONSTRAINT dexcom_history_pkey PRIMARY KEY (date, "time");


--
-- Name: dexcom_manualdata dexcom_manualData_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dexcom_manualdata
    ADD CONSTRAINT "dexcom_manualData_pkey" PRIMARY KEY (pid);


--
-- Name: discarchivator_joblist discarchivator_joblist_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.discarchivator_joblist
    ADD CONSTRAINT discarchivator_joblist_pk PRIMARY KEY (id);


--
-- Name: dump_gb_posts dump_gb_posts_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_gb_posts
    ADD CONSTRAINT dump_gb_posts_pk PRIMARY KEY (id);


--
-- Name: dump_gb_posttags dump_gb_posttags_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_gb_posttags
    ADD CONSTRAINT dump_gb_posttags_pk PRIMARY KEY (postid, tagid);


--
-- Name: dump_gb_tags dump_gb_tags_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_gb_tags
    ADD CONSTRAINT dump_gb_tags_pk PRIMARY KEY (id);


--
-- Name: dump_myfigurecollection_0dumpmeta dump_myfigurecollection_0dumpmeta_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_0dumpmeta
    ADD CONSTRAINT dump_myfigurecollection_0dumpmeta_pk PRIMARY KEY (id);


--
-- Name: dump_myfigurecollection_0statistics dump_myfigurecollection_0statistics_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_0statistics
    ADD CONSTRAINT dump_myfigurecollection_0statistics_pk PRIMARY KEY (id);


--
-- Name: dump_myfigurecollection_categories dump_myfigurecollection_categories_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_categories
    ADD CONSTRAINT dump_myfigurecollection_categories_pk PRIMARY KEY (id);


--
-- Name: dump_myfigurecollection_figurephotos dump_myfigurecollection_figurephotos_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_figurephotos
    ADD CONSTRAINT dump_myfigurecollection_figurephotos_pk PRIMARY KEY (id);


--
-- Name: dump_myfigurecollection_figures dump_myfigurecollection_figures_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_figures
    ADD CONSTRAINT dump_myfigurecollection_figures_pk PRIMARY KEY (id);


--
-- Name: dump_myfigurecollection_roots dump_myfigurecollection_roots_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_myfigurecollection_roots
    ADD CONSTRAINT dump_myfigurecollection_roots_pk PRIMARY KEY (id);


--
-- Name: dump_psxdatacenter_companies dump_psxdatacenter_companies_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_companies
    ADD CONSTRAINT dump_psxdatacenter_companies_pk PRIMARY KEY (id);


--
-- Name: dump_psxdatacenter_game_screenshots dump_psxdatacenter_game_screenshots_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_game_screenshots
    ADD CONSTRAINT dump_psxdatacenter_game_screenshots_pk PRIMARY KEY (hibernateid);


--
-- Name: dump_psxdatacenter_games dump_psxdatacenter_games_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_games
    ADD CONSTRAINT dump_psxdatacenter_games_pk PRIMARY KEY (id);


--
-- Name: dump_psxdatacenter_genres dump_psxdatacenter_genres_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_genres
    ADD CONSTRAINT dump_psxdatacenter_genres_pk PRIMARY KEY (id);


--
-- Name: dump_psxdatacenter_screenshots dump_psxdatacenter_screenshotdata_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_psxdatacenter_screenshots
    ADD CONSTRAINT dump_psxdatacenter_screenshotdata_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_0errors dump_vgmdb_0errors_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_0errors
    ADD CONSTRAINT dump_vgmdb_0errors_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_0statistics dump_vgmdb_0statistics_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_0statistics
    ADD CONSTRAINT dump_vgmdb_0statistics_pk PRIMARY KEY (dateadded);


--
-- Name: dump_vgmdb_album_arbituaryproducts dump_vgmdb_album_arbituaryproducts_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_arbituaryproducts
    ADD CONSTRAINT dump_vgmdb_album_arbituaryproducts_pk UNIQUE (albumid, ordinal);


--
-- Name: dump_vgmdb_album_artist_arbitrary dump_vgmdb_album_artist_arbitrary_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_artist_arbitrary
    ADD CONSTRAINT dump_vgmdb_album_artist_arbitrary_pk PRIMARY KEY (albumid, artisttypeid, name);


--
-- Name: dump_vgmdb_album_artist_type dump_vgmdb_album_artist_type_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_artist_type
    ADD CONSTRAINT dump_vgmdb_album_artist_type_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_album_artists dump_vgmdb_album_artists_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_artists
    ADD CONSTRAINT dump_vgmdb_album_artists_pk PRIMARY KEY (albumid, artistid, artisttypeid);


--
-- Name: dump_vgmdb_album_classification dump_vgmdb_album_classification_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_classification
    ADD CONSTRAINT dump_vgmdb_album_classification_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_album_cover dump_vgmdb_album_cover_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_cover
    ADD CONSTRAINT dump_vgmdb_album_cover_pk PRIMARY KEY (albumid, covername, ordinal);


--
-- Name: dump_vgmdb_album_disc_track_translation dump_vgmdb_album_disc_track_translation_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_disc_track_translation
    ADD CONSTRAINT dump_vgmdb_album_disc_track_translation_pk PRIMARY KEY (albumid, discindex, trackindex, lang);


--
-- Name: dump_vgmdb_album_discs dump_vgmdb_album_discs_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_discs
    ADD CONSTRAINT dump_vgmdb_album_discs_pk PRIMARY KEY (albumid, discindex);


--
-- Name: dump_vgmdb_album_label_arbiturary dump_vgmdb_album_label_arbiturary_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_label_arbiturary
    ADD CONSTRAINT dump_vgmdb_album_label_arbiturary_pk PRIMARY KEY (albumid, ordinal);


--
-- Name: dump_vgmdb_album_label_roles dump_vgmdb_album_label_roles_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_label_roles
    ADD CONSTRAINT dump_vgmdb_album_label_roles_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_album_labels dump_vgmdb_album_labels_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_labels
    ADD CONSTRAINT dump_vgmdb_album_labels_pk PRIMARY KEY (albumid, labelid, roleid);


--
-- Name: dump_vgmdb_album_mediaformat dump_vgmdb_album_mediaformat_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_mediaformat
    ADD CONSTRAINT dump_vgmdb_album_mediaformat_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_album_relatedalbum dump_vgmdb_album_relatedalbum_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_relatedalbum
    ADD CONSTRAINT dump_vgmdb_album_relatedalbum_pk PRIMARY KEY (albumid, relatedalbumid);


--
-- Name: dump_vgmdb_album_releaseevent dump_vgmdb_album_releaseevent_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_releaseevent
    ADD CONSTRAINT dump_vgmdb_album_releaseevent_pk PRIMARY KEY (albumid, eventid);


--
-- Name: dump_vgmdb_album_reprints dump_vgmdb_album_reprints_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_reprints
    ADD CONSTRAINT dump_vgmdb_album_reprints_pk PRIMARY KEY (albumid, reprintid);


--
-- Name: dump_vgmdb_album_titles dump_vgmdb_album_titles_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_titles
    ADD CONSTRAINT dump_vgmdb_album_titles_pk PRIMARY KEY (albumid, langname);


--
-- Name: dump_vgmdb_album_disc_tracks dump_vgmdb_album_tracks_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_disc_tracks
    ADD CONSTRAINT dump_vgmdb_album_tracks_pk PRIMARY KEY (albumid, discindex, trackindex);


--
-- Name: dump_vgmdb_album_types dump_vgmdb_album_type_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_types
    ADD CONSTRAINT dump_vgmdb_album_type_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_album_websites dump_vgmdb_album_websites_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_album_websites
    ADD CONSTRAINT dump_vgmdb_album_websites_pk PRIMARY KEY (albumid, catalog, name, link);


--
-- Name: dump_vgmdb_albums dump_vgmdb_albums_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_albums
    ADD CONSTRAINT dump_vgmdb_albums_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_artist_featured dump_vgmdb_artist_albums_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_artist_featured
    ADD CONSTRAINT dump_vgmdb_artist_albums_pk PRIMARY KEY (artistid, albumid);


--
-- Name: dump_vgmdb_artist_alias dump_vgmdb_artist_alias_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_artist_alias
    ADD CONSTRAINT dump_vgmdb_artist_alias_pk PRIMARY KEY (artistid, ordinal, lang);


--
-- Name: dump_vgmdb_artist dump_vgmdb_artist_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_artist
    ADD CONSTRAINT dump_vgmdb_artist_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_artist_type dump_vgmdb_artist_type_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_artist_type
    ADD CONSTRAINT dump_vgmdb_artist_type_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_artist_websites dump_vgmdb_artist_websites_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_artist_websites
    ADD CONSTRAINT dump_vgmdb_artist_websites_pk PRIMARY KEY (artistid, catalog, name, link);


--
-- Name: dump_vgmdb_events dump_vgmdb_events_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_events
    ADD CONSTRAINT dump_vgmdb_events_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_events_translation dump_vgmdb_events_translation_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_events_translation
    ADD CONSTRAINT dump_vgmdb_events_translation_pk PRIMARY KEY (id, lang);


--
-- Name: dump_vgmdb_label_regions dump_vgmdb_label_regions_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_label_regions
    ADD CONSTRAINT dump_vgmdb_label_regions_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_label_releases dump_vgmdb_label_releases_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_label_releases
    ADD CONSTRAINT dump_vgmdb_label_releases_pk PRIMARY KEY (labelid, albumid);


--
-- Name: dump_vgmdb_label_staff dump_vgmdb_label_staff_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_label_staff
    ADD CONSTRAINT dump_vgmdb_label_staff_pk PRIMARY KEY (labelid, artistid);


--
-- Name: dump_vgmdb_label_types dump_vgmdb_label_types_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_label_types
    ADD CONSTRAINT dump_vgmdb_label_types_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_label_websites dump_vgmdb_label_websites_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_label_websites
    ADD CONSTRAINT dump_vgmdb_label_websites_pk PRIMARY KEY (labelid, catalog, name, link);


--
-- Name: dump_vgmdb_labels dump_vgmdb_labels_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_labels
    ADD CONSTRAINT dump_vgmdb_labels_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_product_albums dump_vgmdb_product_albums_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_albums
    ADD CONSTRAINT dump_vgmdb_product_albums_pk PRIMARY KEY (productid, albumid);


--
-- Name: dump_vgmdb_product_labels dump_vgmdb_product_labels_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_labels
    ADD CONSTRAINT dump_vgmdb_product_labels_pk PRIMARY KEY (productid, labelid);


--
-- Name: dump_vgmdb_product_release_albums dump_vgmdb_product_release_albums_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_albums
    ADD CONSTRAINT dump_vgmdb_product_release_albums_pk PRIMARY KEY (releaseid, albumid);


--
-- Name: dump_vgmdb_product_release_arbitaries dump_vgmdb_product_release_arbitaries_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_arbitaries
    ADD CONSTRAINT dump_vgmdb_product_release_arbitaries_pk PRIMARY KEY (productid, arrayindex, key);


--
-- Name: dump_vgmdb_product_release_platforms dump_vgmdb_product_release_platforms_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_platforms
    ADD CONSTRAINT dump_vgmdb_product_release_platforms_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_product_release_regions dump_vgmdb_product_release_regions_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_regions
    ADD CONSTRAINT dump_vgmdb_product_release_regions_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_product_release_translations dump_vgmdb_product_release_translations_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_translations
    ADD CONSTRAINT dump_vgmdb_product_release_translations_pk PRIMARY KEY (id, lang);


--
-- Name: dump_vgmdb_product_release_types dump_vgmdb_product_release_types_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_release_types
    ADD CONSTRAINT dump_vgmdb_product_release_types_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_product_releases dump_vgmdb_product_releases_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_releases
    ADD CONSTRAINT dump_vgmdb_product_releases_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_product_types dump_vgmdb_product_types_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_types
    ADD CONSTRAINT dump_vgmdb_product_types_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_product_websites dump_vgmdb_product_websites_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_product_websites
    ADD CONSTRAINT dump_vgmdb_product_websites_pk PRIMARY KEY (productid, catalog, name, link);


--
-- Name: dump_vgmdb_products dump_vgmdb_products_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_products
    ADD CONSTRAINT dump_vgmdb_products_pk PRIMARY KEY (id);


--
-- Name: dump_vgmdb_0dumpmeta dump_vndb_0dumpmeta_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_0dumpmeta
    ADD CONSTRAINT dump_vndb_0dumpmeta_pk PRIMARY KEY (id);


--
-- Name: dump_vndb_character_instances dump_vndb_character_instances_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_character_instances
    ADD CONSTRAINT dump_vndb_character_instances_pk PRIMARY KEY (cid, id);


--
-- Name: dump_vndb_character dump_vndb_character_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_character
    ADD CONSTRAINT dump_vndb_character_pk PRIMARY KEY (id);


--
-- Name: dump_vndb_character_traits dump_vndb_character_traits_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_character_traits
    ADD CONSTRAINT dump_vndb_character_traits_pk PRIMARY KEY (cid, tid);


--
-- Name: dump_vndb_character_vns dump_vndb_character_vns_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_character_vns
    ADD CONSTRAINT dump_vndb_character_vns_pk PRIMARY KEY (cid, vnid);


--
-- Name: dump_vndb_character_voiced dump_vndb_character_voiced_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_character_voiced
    ADD CONSTRAINT dump_vndb_character_voiced_pk PRIMARY KEY (relationid);


--
-- Name: dump_vndb_release_languages dump_vndb_release_languages_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_release_languages
    ADD CONSTRAINT dump_vndb_release_languages_pk PRIMARY KEY (rid, lang);


--
-- Name: dump_vndb_release_media dump_vndb_release_media_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_release_media
    ADD CONSTRAINT dump_vndb_release_media_pk PRIMARY KEY (rid, medium);


--
-- Name: dump_vndb_release dump_vndb_release_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_release
    ADD CONSTRAINT dump_vndb_release_pk PRIMARY KEY (id);


--
-- Name: dump_vndb_release_platforms dump_vndb_release_platforms_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_release_platforms
    ADD CONSTRAINT dump_vndb_release_platforms_pk PRIMARY KEY (rid, platform);


--
-- Name: dump_vndb_release_producers dump_vndb_release_producers_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_release_producers
    ADD CONSTRAINT dump_vndb_release_producers_pk PRIMARY KEY (rid, pid);


--
-- Name: dump_vndb_release_vns dump_vndb_release_vns_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_release_vns
    ADD CONSTRAINT dump_vndb_release_vns_pk PRIMARY KEY (rid, vnid);


--
-- Name: dump_vndb_tags_aliases dump_vndb_tags_aliases_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_tags_aliases
    ADD CONSTRAINT dump_vndb_tags_aliases_pk PRIMARY KEY (tagid, name);


--
-- Name: dump_vndb_tags_parents dump_vndb_tags_parents_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_tags_parents
    ADD CONSTRAINT dump_vndb_tags_parents_pk PRIMARY KEY (child, parent);


--
-- Name: dump_vndb_tags dump_vndb_tags_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_tags
    ADD CONSTRAINT dump_vndb_tags_pk PRIMARY KEY (id);


--
-- Name: dump_vndb_traits_aliases dump_vndb_traits_aliases_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_traits_aliases
    ADD CONSTRAINT dump_vndb_traits_aliases_pk PRIMARY KEY (id, alias);


--
-- Name: dump_vndb_traits_parents dump_vndb_traits_parents_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_traits_parents
    ADD CONSTRAINT dump_vndb_traits_parents_pk PRIMARY KEY (child, parent);


--
-- Name: dump_vndb_vn_anime dump_vndb_vn_anime_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_anime
    ADD CONSTRAINT dump_vndb_vn_anime_pk PRIMARY KEY (relationid);


--
-- Name: dump_vndb_vn_languages dump_vndb_vn_languages_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_languages
    ADD CONSTRAINT dump_vndb_vn_languages_pk PRIMARY KEY (vnid, language, orig_lang);


--
-- Name: dump_vndb_vn dump_vndb_vn_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn
    ADD CONSTRAINT dump_vndb_vn_pk PRIMARY KEY (id);


--
-- Name: dump_vndb_vn_platforms dump_vndb_vn_platforms_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_platforms
    ADD CONSTRAINT dump_vndb_vn_platforms_pk PRIMARY KEY (vnid, platform);


--
-- Name: dump_vndb_vn_relation dump_vndb_vn_relation_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_relation
    ADD CONSTRAINT dump_vndb_vn_relation_pk PRIMARY KEY (srcid, id);


--
-- Name: dump_vndb_vn_screens dump_vndb_vn_screens_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_screens
    ADD CONSTRAINT dump_vndb_vn_screens_pk PRIMARY KEY (screenid);


--
-- Name: dump_vndb_vn_staff dump_vndb_vn_staff_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_staff
    ADD CONSTRAINT dump_vndb_vn_staff_pk PRIMARY KEY (id);


--
-- Name: dump_vndb_vn_tag dump_vndb_vn_tag_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_vn_tag
    ADD CONSTRAINT dump_vndb_vn_tag_pk PRIMARY KEY (srcid, tagid);


--
-- Name: dump_vndb_votes dump_vndb_votes_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_votes
    ADD CONSTRAINT dump_vndb_votes_pk PRIMARY KEY (vnid, userid);


--
-- Name: dump_vocadb_0banevasion dump_vocadb_0banevasion_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vocadb_0banevasion
    ADD CONSTRAINT dump_vocadb_0banevasion_pk PRIMARY KEY (id);


--
-- Name: dump_vocadb_0dumpmeta dump_vocadb_0dumpmeta_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vocadb_0dumpmeta
    ADD CONSTRAINT dump_vocadb_0dumpmeta_pk PRIMARY KEY (id);


--
-- Name: dump_vocadb_albums dump_vocadb_albums_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vocadb_albums
    ADD CONSTRAINT dump_vocadb_albums_pk PRIMARY KEY (id);


--
-- Name: dump_vocadb_albumtracks dump_vocadb_albumtracks_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vocadb_albumtracks
    ADD CONSTRAINT dump_vocadb_albumtracks_pk PRIMARY KEY (id);


--
-- Name: dump_vocadb_songartists dump_vocadb_trackartists_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vocadb_songartists
    ADD CONSTRAINT dump_vocadb_trackartists_pk PRIMARY KEY (id);


--
-- Name: licensing_fatclient_machines licensing_machines_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.licensing_fatclient_machines
    ADD CONSTRAINT licensing_machines_pk PRIMARY KEY (uid);


--
-- Name: mailarchive_folders mailarchive_folders_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.mailarchive_folders
    ADD CONSTRAINT mailarchive_folders_pkey PRIMARY KEY (id);


--
-- Name: mailarchive_mails mailarchive_mails_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.mailarchive_mails
    ADD CONSTRAINT mailarchive_mails_pkey PRIMARY KEY (uid);


--
-- Name: notebook_notes notebook_notes_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.notebook_notes
    ADD CONSTRAINT notebook_notes_pk PRIMARY KEY (id);


--
-- Name: osm_node_tags osm_node_tags_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.osm_node_tags
    ADD CONSTRAINT osm_node_tags_pkey PRIMARY KEY (k, "nodeId");


--
-- Name: osm_nodes osm_nodes_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.osm_nodes
    ADD CONSTRAINT osm_nodes_pkey PRIMARY KEY (id);


--
-- Name: osm_relation_tags osm_relation_tags_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.osm_relation_tags
    ADD CONSTRAINT osm_relation_tags_pkey PRIMARY KEY (k, "relationId");


--
-- Name: osm_relations osm_relations_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.osm_relations
    ADD CONSTRAINT osm_relations_pkey PRIMARY KEY (id);


--
-- Name: osm_way_nodes osm_way_nodes_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.osm_way_nodes
    ADD CONSTRAINT osm_way_nodes_pkey PRIMARY KEY (node, way);


--
-- Name: osm_way_tags osm_way_tags_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.osm_way_tags
    ADD CONSTRAINT osm_way_tags_pkey PRIMARY KEY (k, "wayId");


--
-- Name: osm_ways osm_ways_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.osm_ways
    ADD CONSTRAINT osm_ways_pkey PRIMARY KEY (id);


--
-- Name: patchouli_backupsets patchouli_backupsets_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_backupsets
    ADD CONSTRAINT patchouli_backupsets_pk PRIMARY KEY (id);


--
-- Name: patchouli_filecatalog patchouli_filecatalog_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_filecatalog
    ADD CONSTRAINT patchouli_filecatalog_pk PRIMARY KEY (id);


--
-- Name: patchouli_machines patchouli_machines_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_machines
    ADD CONSTRAINT patchouli_machines_pk PRIMARY KEY (id);


--
-- Name: patchouli_smart_disks patchouli_smart_disks_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_smart_disks
    ADD CONSTRAINT patchouli_smart_disks_pk PRIMARY KEY (id);


--
-- Name: patchouli_smart_values patchouli_smart_values_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_smart_values
    ADD CONSTRAINT patchouli_smart_values_pk PRIMARY KEY (dateadded, driveid);


--
-- Name: patchouli_tapes patchouli_tapes_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.patchouli_tapes
    ADD CONSTRAINT patchouli_tapes_pk PRIMARY KEY (id);


--
-- Name: sedgetree_photos sedgetree_photos_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.sedgetree_photos
    ADD CONSTRAINT sedgetree_photos_pkey PRIMARY KEY (person);


--
-- Name: sedgetree_versioning sedgetree_versioning_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.sedgetree_versioning
    ADD CONSTRAINT sedgetree_versioning_pkey PRIMARY KEY (id);


--
-- Name: tarbuddy_schedule tarbuddy_schedule_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.tarbuddy_schedule
    ADD CONSTRAINT tarbuddy_schedule_pkey PRIMARY KEY (date);


--
-- Name: dump_vgmdb_event_releases vgmdb_dump_event_releases_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vgmdb_event_releases
    ADD CONSTRAINT vgmdb_dump_event_releases_pk PRIMARY KEY (eventid, albumid);


--
-- Name: warwalking_discoveries warwalking_discoveries_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.warwalking_discoveries
    ADD CONSTRAINT warwalking_discoveries_pkey PRIMARY KEY (bssid);


--
-- Name: warwalking_tours warwalking_tours_pkey; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.warwalking_tours
    ADD CONSTRAINT warwalking_tours_pkey PRIMARY KEY (id);


--
-- Name: web_beacon web_beacon_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.web_beacon
    ADD CONSTRAINT web_beacon_pk PRIMARY KEY (id);


--
-- Name: web_heartbeat web_heartbeat_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.web_heartbeat
    ADD CONSTRAINT web_heartbeat_pk PRIMARY KEY (id);


--
-- Name: web_startup web_startup_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.web_startup
    ADD CONSTRAINT web_startup_pk PRIMARY KEY (id);


--
-- Name: web_users web_users_pk; Type: CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.web_users
    ADD CONSTRAINT web_users_pk PRIMARY KEY (id);


--
-- Name: FK__shelves; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "FK__shelves" ON public.azusa_products USING btree (inshelf);


--
-- Name: FK_media_products; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "FK_media_products" ON public.azusa_media USING btree (relatedproduct);


--
-- Name: azusa_filesysteminfo_mediaid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX azusa_filesysteminfo_mediaid_index ON public.azusa_filesysteminfo USING btree (mediaid);


--
-- Name: azusa_media_dateAdded2_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "azusa_media_dateAdded2_index" ON public.azusa_media USING btree (dateadded);


--
-- Name: cddb_disc_sqlid_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX cddb_disc_sqlid_uindex ON public.cddb_disc USING btree (sqlid);


--
-- Name: cddb_track_dateadded_index; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX cddb_track_dateadded_index ON public.cddb_track USING btree (dateadded DESC);


--
-- Name: dexcom_history_date_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dexcom_history_date_index ON public.dexcom_history USING btree (date);


--
-- Name: dump_gb_posttags_postid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_gb_posttags_postid_index ON public.dump_gb_posttags USING btree (postid);


--
-- Name: dump_gb_posttags_tagid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_gb_posttags_tagid_index ON public.dump_gb_posttags USING btree (tagid);


--
-- Name: dump_psxdatacenter_game_screenshots_gameid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_psxdatacenter_game_screenshots_gameid_index ON public.dump_psxdatacenter_game_screenshots USING btree (gameid);


--
-- Name: dump_psxdatacenter_games_sku_uindex; Type: INDEX; Schema: public; Owner: ft
--

CREATE UNIQUE INDEX dump_psxdatacenter_games_sku_uindex ON public.dump_psxdatacenter_games USING btree (sku);


--
-- Name: dump_vgmdb_labels_name_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vgmdb_labels_name_index ON public.dump_vgmdb_labels USING btree (name);


--
-- Name: dump_vgmdb_product_websites_productid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vgmdb_product_websites_productid_index ON public.dump_vgmdb_product_websites USING btree (productid);


--
-- Name: dump_vndb_tags_parents_child_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vndb_tags_parents_child_index ON public.dump_vndb_tags_parents USING btree (child);


--
-- Name: dump_vndb_traits_aliases_id_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vndb_traits_aliases_id_index ON public.dump_vndb_traits_aliases USING btree (id);


--
-- Name: dump_vndb_traits_parents_child_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vndb_traits_parents_child_index ON public.dump_vndb_traits_parents USING btree (child);


--
-- Name: dump_vndb_votes_vnid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vndb_votes_vnid_index ON public.dump_vndb_votes USING btree (vnid);


--
-- Name: dump_vocadb_albums_name_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vocadb_albums_name_index ON public.dump_vocadb_albums USING btree (name);


--
-- Name: dump_vocadb_albumtracks_albumid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX dump_vocadb_albumtracks_albumid_index ON public.dump_vocadb_albumtracks USING btree (albumid DESC);


--
-- Name: folderIndex; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "folderIndex" ON public.mailarchive_mails USING btree (folder);


--
-- Name: mainindex; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX mainindex ON public.discarchivator_joblist USING btree (volumelabel, discid, vts, pgc);


--
-- Name: mediaTypes_shortName_uindex; Type: INDEX; Schema: public; Owner: ft
--

CREATE UNIQUE INDEX "mediaTypes_shortName_uindex" ON public.azusa_mediatypes USING btree ("shortName");


--
-- Name: messageTimeIndex; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "messageTimeIndex" ON public.mailarchive_mails USING btree ("messageUtime");


--
-- Name: nodes_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "nodes_dateAdded_index" ON public.osm_nodes USING btree ("dateAdded");


--
-- Name: nodes_lat_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX nodes_lat_index ON public.osm_nodes USING btree (lat);


--
-- Name: nodes_lon_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX nodes_lon_index ON public.osm_nodes USING btree (lon);


--
-- Name: nodetags_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "nodetags_dateAdded_index" ON public.osm_node_tags USING btree ("dateAdded");


--
-- Name: relation_member_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "relation_member_dateAdded_index" ON public.osm_relation_member USING btree ("dateAdded");


--
-- Name: relation_member_relationId_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "relation_member_relationId_index" ON public.osm_relation_member USING btree ("relationId");


--
-- Name: relation_tags_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "relation_tags_dateAdded_index" ON public.osm_relation_tags USING btree ("dateAdded");


--
-- Name: relations_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "relations_dateAdded_index" ON public.osm_relations USING btree ("dateAdded");


--
-- Name: tourIndex; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "tourIndex" ON public.warwalking_discoveries USING btree ("strongestTour");


--
-- Name: warwalking_discoveries_bssid_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX warwalking_discoveries_bssid_index ON public.warwalking_discoveries USING btree (bssid);


--
-- Name: way_nodes_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "way_nodes_dateAdded_index" ON public.osm_way_nodes USING btree ("dateAdded");


--
-- Name: way_tags_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "way_tags_dateAdded_index" ON public.osm_way_tags USING btree ("dateAdded");


--
-- Name: ways_dateAdded_index; Type: INDEX; Schema: public; Owner: ft
--

CREATE INDEX "ways_dateAdded_index" ON public.osm_ways USING btree ("dateAdded");


--
-- Name: web_beacon_name_uindex; Type: INDEX; Schema: public; Owner: ft
--

CREATE UNIQUE INDEX web_beacon_name_uindex ON public.web_beacon USING btree (name);


--
-- Name: dump_vndb_tags_aliases dump_vndb_tags_aliases_dump_vndb_tags_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: ft
--

ALTER TABLE ONLY public.dump_vndb_tags_aliases
    ADD CONSTRAINT dump_vndb_tags_aliases_dump_vndb_tags_id_fk FOREIGN KEY (tagid) REFERENCES public.dump_vndb_tags(id);


--
-- Name: TABLE discarchivator_tasklog; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.discarchivator_tasklog TO PUBLIC;
GRANT ALL ON TABLE public.discarchivator_tasklog TO ft;


--
-- PostgreSQL database dump complete
--

