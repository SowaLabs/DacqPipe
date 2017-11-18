--
-- PostgreSQL database dump
--

-- Dumped from database version 9.6.5
-- Dumped by pg_dump version 9.6.5

-- Started on 2017-10-27 11:20:45

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12387)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2130 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 185 (class 1259 OID 16753)
-- Name: Documents; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "Documents" (
    guid uuid,
    hash uuid,
    title character varying(400),
    description character varying(400),
    snippet character varying(1000),
    category character varying(400),
    link character varying(400),
    "responseUrl" character varying(400),
    "urlKey" character varying(400),
    "time" timestamp without time zone,
    "pubDate" character varying(100),
    "mimeType" character varying(80),
    "charSet" character varying(40),
    "contentLength" integer,
    "domainName" character varying(100),
    "bprBoilerplateCharCount" integer,
    "bprContentCharCount" integer,
    "unseenContentCharCount" integer,
    rev integer,
    "fileName" character varying(100),
    "siteId" character varying(100)
);


ALTER TABLE "Documents" OWNER TO postgres;

--
-- TOC entry 186 (class 1259 OID 16760)
-- Name: TextBlocks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE "TextBlocks" (
    "docGuid" uuid NOT NULL,
    "hashCodes" bytea NOT NULL
);


ALTER TABLE "TextBlocks" OWNER TO postgres;

--
-- TOC entry 2005 (class 1259 OID 46463)
-- Name: IDX_siteId_time; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IDX_siteId_time" ON "Documents" USING btree ("siteId", title DESC NULLS LAST);


--
-- TOC entry 2006 (class 1259 OID 16759)
-- Name: UQ_guid; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "UQ_guid" ON "Documents" USING btree (guid);


-- Completed on 2017-10-27 11:20:46

--
-- PostgreSQL database dump complete
--

