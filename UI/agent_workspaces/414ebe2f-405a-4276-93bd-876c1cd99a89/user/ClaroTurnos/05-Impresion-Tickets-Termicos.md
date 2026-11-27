# Procedimiento de impresión de tickets térmicos

## 1. Hardware de referencia
Impresoras térmicas de recibo compatibles con **ESC/POS** (ej. Epson TM-T20/T88, Bixolon SRP-350, o genéricas 58/80 mm), conectadas por Ethernet/IP, USB o Bluetooth al kiosco.

## 2. Flujo de impresión

```
Kiosco (React) → POST /api/tickets  (crea ticket, responde con DTO)
                → llama a servicio local de impresión
                        (Web Serial API / agente local ligero en el kiosco)
                → o el backend envía el trabajo directamente a la IP de la
                  impresora configurada en ImpresorasTermicas (recomendado
                  cuando el kiosco es un thin client sin drivers)
```

Recomendación para el MVP: **el backend genera el buffer ESC/POS y lo envía por socket TCP al puerto 9100** de la impresora IP configurada en `ImpresorasTermicas`. Esto evita instalar drivers en cada kiosco y centraliza la lógica de plantillas.

## 3. Plantilla del ticket

```
      [LOGO CLARO - centrado, bitmap monocromo 200x60px]

              A - 001
          --------------------
             VENTAS
      20/08/2026   09:15 AM
      --------------------
      Personas antes de usted: 4
      Sucursal Principal

      Gracias por preferir Claro
      Conserve este ticket
      |||  ||  |  ||| ||  ||||   (pseudo-barcode decorativo)
```

## 4. Ejemplo de servicio de impresión (C#, capa Infrastructure)

```csharp
// ClaroTurnos.Infrastructure/Printing/EscPosPrinterService.cs
public class EscPosPrinterService : IPrinterService
{
    public async Task ImprimirTicketAsync(ImpresoraTermica impresora, TicketImpresionDto ticket)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(impresora.DireccionIP, impresora.Puerto ?? 9100);
        using var stream = client.GetStream();

        var buffer = new List<byte>();
        buffer.AddRange(Escpos.Init());                       // ESC @
        buffer.AddRange(Escpos.AlinearCentro());
        buffer.AddRange(Escpos.ImprimirLogo());                // logo pre-cargado en la impresora (NV bitmap)
        buffer.AddRange(Escpos.FuenteGrandeNegrita());
        buffer.AddRange(Escpos.Texto($"{ticket.Codigo}\n"));   // A-001
        buffer.AddRange(Escpos.FuenteNormal());
        buffer.AddRange(Escpos.Texto($"{ticket.ServicioNombre}\n"));
        buffer.AddRange(Escpos.Texto($"{ticket.Fecha:dd/MM/yyyy hh:mm tt}\n"));
        buffer.AddRange(Escpos.LineaPunteada());
        buffer.AddRange(Escpos.Texto($"Personas antes de usted: {ticket.PersonasEnEspera}\n"));
        buffer.AddRange(Escpos.Texto($"{ticket.SucursalNombre}\n\n"));
        buffer.AddRange(Escpos.Texto("Gracias por preferir Claro\n"));
        buffer.AddRange(Escpos.CorteParcial());                // GS V

        await stream.WriteAsync(buffer.ToArray());
    }
}
```

> En producción se recomienda usar una librería madura (`ESCPOS.NET` o `EscPosPrinter`) en lugar de construir el buffer a mano; el ejemplo anterior ilustra la secuencia lógica de comandos.

## 5. Manejo de fallas
- Si la impresora no responde (timeout de conexión), el ticket se marca igualmente como generado en base de datos (la cola no depende de la impresión física) y se registra un evento `ERROR` en `LogsEventos` con origen `Impresora`.
- El Kiosco muestra un mensaje: *"Su turno fue registrado: A-001. Hubo un problema al imprimir, tome nota de su número."*
- El Panel Administrativo muestra alertas de impresoras fuera de línea (ping periódico / último trabajo exitoso).

## 6. Configuración por sucursal
Cada registro de `ImpresorasTermicas` define: kiosco asociado, tipo de conexión, IP/puerto o identificador USB, ancho de papel (58/80mm) y la plantilla de texto (para permitir variaciones de copy por sucursal sin desplegar código).
