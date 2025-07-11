using System.Text.RegularExpressions;

namespace BackendSport.API.Middleware;

/// <summary>
/// Middleware para sanitizar inputs y prevenir ataques de inyección.
/// Implementa detección y limpieza de contenido malicioso en query parameters y headers.
/// </summary>
/// <remarks>
/// Este middleware protege contra:
/// - Cross-Site Scripting (XSS)
/// - HTML injection
/// - JavaScript injection
/// - CSS injection
/// - Header injection attacks
/// 
/// Utiliza expresiones regulares para detectar patrones peligrosos y los elimina
/// o registra intentos de ataque para análisis posterior.
/// </remarks>
public class InputSanitizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<InputSanitizationMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del middleware de sanitización de inputs.
    /// </summary>
    /// <param name="next">Siguiente middleware en el pipeline</param>
    /// <param name="logger">Logger para registrar intentos de ataque</param>
    public InputSanitizationMiddleware(RequestDelegate next, ILogger<InputSanitizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Procesa la solicitud HTTP sanitizando inputs potencialmente peligrosos.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP</param>
    /// <returns>Task que representa la operación asíncrona</returns>
    /// <remarks>
    /// El middleware sanitiza:
    /// 1. Query parameters de la URL
    /// 2. Headers HTTP sospechosos
    /// 
    /// Si se detecta contenido malicioso, se registra en el log para análisis.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        // Sanitizar query parameters
        if (context.Request.QueryString.HasValue)
        {
            var sanitizedQuery = SanitizeQueryString(context.Request.QueryString.Value);
            context.Request.QueryString = new QueryString(sanitizedQuery);
        }

        // Sanitizar headers sospechosos
        SanitizeHeaders(context.Request.Headers);

        await _next(context);
    }

    /// <summary>
    /// Sanitiza una cadena de query parameters eliminando contenido malicioso.
    /// </summary>
    /// <param name="queryString">Cadena de query parameters a sanitizar</param>
    /// <returns>Query string sanitizada</returns>
    /// <remarks>
    /// Detecta y elimina patrones peligrosos como:
    /// - Tags HTML maliciosos
    /// - JavaScript injection
    /// - CSS injection
    /// - Event handlers
    /// - Protocolos peligrosos (javascript:, vbscript:)
    /// 
    /// Si se detecta contenido malicioso, se registra en el log.
    /// </remarks>
    private string SanitizeQueryString(string queryString)
    {
        if (string.IsNullOrEmpty(queryString))
            return queryString;

        // Patrones peligrosos a detectar
        var dangerousPatterns = new[]
        {
            @"<script[^>]*>.*?</script>",
            @"javascript:",
            @"vbscript:",
            @"onload\s*=",
            @"onerror\s*=",
            @"onclick\s*=",
            @"<iframe[^>]*>",
            @"<object[^>]*>",
            @"<embed[^>]*>",
            @"<form[^>]*>",
            @"<input[^>]*>",
            @"<textarea[^>]*>",
            @"<select[^>]*>",
            @"<button[^>]*>",
            @"<link[^>]*>",
            @"<meta[^>]*>",
            @"<style[^>]*>",
            @"<title[^>]*>",
            @"<body[^>]*>",
            @"<html[^>]*>",
            @"<head[^>]*>",
            @"<div[^>]*>",
            @"<span[^>]*>",
            @"<p[^>]*>",
            @"<br[^>]*>",
            @"<hr[^>]*>",
            @"<h[1-6][^>]*>",
            @"<ul[^>]*>",
            @"<ol[^>]*>",
            @"<li[^>]*>",
            @"<dl[^>]*>",
            @"<dt[^>]*>",
            @"<dd[^>]*>",
            @"<table[^>]*>",
            @"<tr[^>]*>",
            @"<td[^>]*>",
            @"<th[^>]*>",
            @"<thead[^>]*>",
            @"<tbody[^>]*>",
            @"<tfoot[^>]*>",
            @"<caption[^>]*>",
            @"<colgroup[^>]*>",
            @"<col[^>]*>",
            @"<fieldset[^>]*>",
            @"<legend[^>]*>",
            @"<label[^>]*>",
            @"<optgroup[^>]*>",
            @"<option[^>]*>",
            @"<textarea[^>]*>",
            @"<output[^>]*>",
            @"<progress[^>]*>",
            @"<meter[^>]*>",
            @"<details[^>]*>",
            @"<summary[^>]*>",
            @"<menu[^>]*>",
            @"<menuitem[^>]*>",
            @"<dialog[^>]*>",
            @"<slot[^>]*>",
            @"<template[^>]*>",
            @"<picture[^>]*>",
            @"<source[^>]*>",
            @"<track[^>]*>",
            @"<map[^>]*>",
            @"<area[^>]*>",
            @"<svg[^>]*>",
            @"<math[^>]*>",
            @"<canvas[^>]*>",
            @"<audio[^>]*>",
            @"<video[^>]*>",
            @"<bdi[^>]*>",
            @"<bdo[^>]*>",
            @"<cite[^>]*>",
            @"<code[^>]*>",
            @"<data[^>]*>",
            @"<dfn[^>]*>",
            @"<em[^>]*>",
            @"<i[^>]*>",
            @"<kbd[^>]*>",
            @"<mark[^>]*>",
            @"<q[^>]*>",
            @"<rp[^>]*>",
            @"<rt[^>]*>",
            @"<ruby[^>]*>",
            @"<s[^>]*>",
            @"<samp[^>]*>",
            @"<small[^>]*>",
            @"<strong[^>]*>",
            @"<sub[^>]*>",
            @"<sup[^>]*>",
            @"<time[^>]*>",
            @"<u[^>]*>",
            @"<var[^>]*>",
            @"<wbr[^>]*>",
            @"<abbr[^>]*>",
            @"<acronym[^>]*>",
            @"<applet[^>]*>",
            @"<basefont[^>]*>",
            @"<big[^>]*>",
            @"<center[^>]*>",
            @"<dir[^>]*>",
            @"<font[^>]*>",
            @"<isindex[^>]*>",
            @"<listing[^>]*>",
            @"<plaintext[^>]*>",
            @"<strike[^>]*>",
            @"<tt[^>]*>",
            @"<xmp[^>]*>"
        };

        var sanitized = queryString;
        foreach (var pattern in dangerousPatterns)
        {
            sanitized = Regex.Replace(sanitized, pattern, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        // Detectar y loggear intentos de ataque
        if (sanitized != queryString)
        {
            _logger.LogWarning("Potential XSS attack detected in query string. Original: {Original}, Sanitized: {Sanitized}", 
                queryString, sanitized);
        }

        return sanitized;
    }

    /// <summary>
    /// Sanitiza headers HTTP sospechosos eliminando contenido malicioso.
    /// </summary>
    /// <param name="headers">Colección de headers HTTP a sanitizar</param>
    /// <remarks>
    /// Verifica headers que pueden ser utilizados para ataques:
    /// - X-Forwarded-* headers
    /// - Headers de proxy
    /// - Headers de reescritura de URL
    /// 
    /// Si se detecta contenido malicioso en estos headers, se eliminan.
    /// </remarks>
    private void SanitizeHeaders(IHeaderDictionary headers)
    {
        var suspiciousHeaders = new[]
        {
            "X-Forwarded-Host",
            "X-Forwarded-Proto",
            "X-Forwarded-For",
            "X-Real-IP",
            "X-Original-URL",
            "X-Rewrite-URL"
        };

        foreach (var header in suspiciousHeaders)
        {
            if (headers.ContainsKey(header))
            {
                var value = headers[header].ToString();
                if (IsSuspiciousHeaderValue(value))
                {
                    _logger.LogWarning("Suspicious header value detected: {Header} = {Value}", header, value);
                    headers.Remove(header);
                }
            }
        }
    }

    /// <summary>
    /// Determina si un valor de header es sospechoso o malicioso.
    /// </summary>
    /// <param name="value">Valor del header a verificar</param>
    /// <returns>true si el valor es sospechoso, false en caso contrario</returns>
    /// <remarks>
    /// Verifica patrones peligrosos similares a los de query parameters:
    /// - Tags HTML
    /// - JavaScript injection
    /// - Event handlers
    /// - Protocolos peligrosos
    /// </remarks>
    private bool IsSuspiciousHeaderValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        var suspiciousPatterns = new[]
        {
            @"<script[^>]*>",
            @"javascript:",
            @"vbscript:",
            @"onload\s*=",
            @"onerror\s*=",
            @"onclick\s*=",
            @"<iframe[^>]*>",
            @"<object[^>]*>",
            @"<embed[^>]*>",
            @"<form[^>]*>",
            @"<input[^>]*>",
            @"<textarea[^>]*>",
            @"<select[^>]*>",
            @"<button[^>]*>",
            @"<link[^>]*>",
            @"<meta[^>]*>",
            @"<style[^>]*>",
            @"<title[^>]*>",
            @"<body[^>]*>",
            @"<html[^>]*>",
            @"<head[^>]*>",
            @"<div[^>]*>",
            @"<span[^>]*>",
            @"<p[^>]*>",
            @"<br[^>]*>",
            @"<hr[^>]*>",
            @"<h[1-6][^>]*>",
            @"<ul[^>]*>",
            @"<ol[^>]*>",
            @"<li[^>]*>",
            @"<dl[^>]*>",
            @"<dt[^>]*>",
            @"<dd[^>]*>",
            @"<table[^>]*>",
            @"<tr[^>]*>",
            @"<td[^>]*>",
            @"<th[^>]*>",
            @"<thead[^>]*>",
            @"<tbody[^>]*>",
            @"<tfoot[^>]*>",
            @"<caption[^>]*>",
            @"<colgroup[^>]*>",
            @"<col[^>]*>",
            @"<fieldset[^>]*>",
            @"<legend[^>]*>",
            @"<label[^>]*>",
            @"<optgroup[^>]*>",
            @"<option[^>]*>",
            @"<textarea[^>]*>",
            @"<output[^>]*>",
            @"<progress[^>]*>",
            @"<meter[^>]*>",
            @"<details[^>]*>",
            @"<summary[^>]*>",
            @"<menu[^>]*>",
            @"<menuitem[^>]*>",
            @"<dialog[^>]*>",
            @"<slot[^>]*>",
            @"<template[^>]*>",
            @"<picture[^>]*>",
            @"<source[^>]*>",
            @"<track[^>]*>",
            @"<map[^>]*>",
            @"<area[^>]*>",
            @"<svg[^>]*>",
            @"<math[^>]*>",
            @"<canvas[^>]*>",
            @"<audio[^>]*>",
            @"<video[^>]*>",
            @"<bdi[^>]*>",
            @"<bdo[^>]*>",
            @"<cite[^>]*>",
            @"<code[^>]*>",
            @"<data[^>]*>",
            @"<dfn[^>]*>",
            @"<em[^>]*>",
            @"<i[^>]*>",
            @"<kbd[^>]*>",
            @"<mark[^>]*>",
            @"<q[^>]*>",
            @"<rp[^>]*>",
            @"<rt[^>]*>",
            @"<ruby[^>]*>",
            @"<s[^>]*>",
            @"<samp[^>]*>",
            @"<small[^>]*>",
            @"<strong[^>]*>",
            @"<sub[^>]*>",
            @"<sup[^>]*>",
            @"<time[^>]*>",
            @"<u[^>]*>",
            @"<var[^>]*>",
            @"<wbr[^>]*>",
            @"<abbr[^>]*>",
            @"<acronym[^>]*>",
            @"<applet[^>]*>",
            @"<basefont[^>]*>",
            @"<big[^>]*>",
            @"<center[^>]*>",
            @"<dir[^>]*>",
            @"<font[^>]*>",
            @"<isindex[^>]*>",
            @"<listing[^>]*>",
            @"<plaintext[^>]*>",
            @"<strike[^>]*>",
            @"<tt[^>]*>",
            @"<xmp[^>]*>"
        };

        return suspiciousPatterns.Any(pattern => Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline));
    }
} 