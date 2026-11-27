//@ts-check
import { WModalForm } from "../WDevCore/WComponents/WModalForm.js";
import { WAjaxTools } from "../WDevCore/WModules/WAjaxTools.js";
import { html, WRender } from "../WDevCore/WModules/WComponentsTools.js";
import { css } from "../WDevCore/WModules/WStyledRender.js";

const MAX_UPLOAD_BYTES = 256 * 1024;
const MAX_FOLDER_FILES = 200;
const ALLOWED_EXT = [".js", ".ts", ".cs", ".json", ".md", ".txt", ".html", ".css", ".sql", ".csv", ".xml", ".py"];
const IGNORED_FOLDERS = ["bin", "obj", "node_modules", ".git"];

export async function AgentGet(url) {
    const res = await fetch(url, { credentials: "include", headers: { "Accept": "application/json" } });
    const data = await res.json().catch(() => null);
    if (!res.ok) throw new Error(data?.error ?? `HTTP ${res.status}`);
    return data;
}
const AgentPost = (url, model) => WAjaxTools.PostRequest(url, model, { WithoutLoading: true });
const ReadAsBase64 = (file) => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => resolve(String(reader.result).split(",")[1] ?? "");
    reader.onerror = reject;
    reader.readAsDataURL(file);
});

export async function OpenRunTraceModal(idRun, userId, apiBase = "../api") {
    const run = await AgentGet(`${apiBase}/AgentRun/${idRun}?userId=${encodeURIComponent(userId)}`);
    const rows = (run.Trace ?? []).map(t => html`
        <div class="trace-step ${t.ok === false ? "fail" : ""}">
            <span class="trace-num">${t.step}</span>
            <span class="trace-action">${t.action}</span>
            <span class="trace-obs">${t.obs ?? ""}</span>
        </div>`);
    document.body.append(new WModalForm({
        ObjectModal: html`<div class="agent-trace">
            ${css`
                .agent-trace { min-width: 480px; max-width: 720px; max-height: 70vh; overflow-y: auto; padding: 12px; }
                .trace-step { display: grid; grid-template-columns: 30px 110px 1fr; gap: 8px; padding: 6px 4px; border-bottom: 1px solid #ddd; font-size: 12px; }
                .trace-step.fail .trace-action { color: #c0392b; font-weight: 600; }
                .trace-num { opacity: .5; } .trace-action { font-weight: 600; }
                .trace-obs { word-break: break-all; opacity: .8; }
                .trace-meta { font-size: 12px; opacity: .6; margin-bottom: 8px; }
            `}
            <h3>Ejecución #${run.Id_Run} — ${run.Skill} (${run.Status})</h3>
            <div class="trace-meta">${run.Steps} pasos · ${run.StartedAt ?? ""} → ${run.EndedAt ?? ""}</div>
            ${rows}
        </div>`
    }));
}

export class WAgentContextPanel extends HTMLElement {
    /** @param {{ Token: string, UserId: string, ApiBase?: string }} Config */
    constructor(Config) {
        super();
        this.Config = Config;
        this.Token = Config.Token;
        this.UserId = Config.UserId;
        this.ApiBase = Config.ApiBase ?? "../api";
        /**@type {string|null} */ this.Skill = localStorage.getItem("AgentSkill") || null;
        /**@type {number|null} */ this.WorkspaceId = null;
        /**@type {string|null} */ this.RootPath = null;
        this.className = "agent-context-panel";

        this.RootLabel = html`<div class="acp-root" title="Carpeta actual del workspace"></div>`;
        this.PathInput = html`<input class="acp-input" placeholder="D:\\proyectos\\mi-app"/>`;
        this.SkillSelect = html`<select class="acp-select"></select>`;
        this.UserBlock = html`<div class="acp-block  files"></div>`;
        this.AgentBlock = html`<div class="acp-block  files"></div>`;
        this.Status = html`<div class="acp-status"></div>`;

        this.FileInput = WRender.Create({ tagName: "input", type: "file", multiple: true });
        this.FileInput.style.display = "none";
        this.FolderInput = WRender.Create({ tagName: "input", type: "file" });
        this.FolderInput.setAttribute("webkitdirectory", "");
        this.FolderInput.style.display = "none";

        const usePathBtn = html`<button class="acp-btn">Usar</button>`;
        const pickFolderBtn = html`<button class="acp-btn acp-wide" title="Diálogo de Windows: sube una copia de la carpeta al workspace">Elegir carpeta del equipo…</button>`;
        const uploadBtn = html`<button class="acp-btn" title="Subir archivos al bloque USUARIO">＋</button>`;
        const runsBtn = html`<button class="acp-btn acp-wide">Historial de ejecuciones</button>`;

        usePathBtn.addEventListener("click", () => this.UsePath());
        this.PathInput.addEventListener("keydown", (e) => { if (e.key === "Enter") this.UsePath(); });
        pickFolderBtn.addEventListener("click", () => this.FolderInput.click());
        uploadBtn.addEventListener("click", () => this.FileInput.click());
        this.FileInput.addEventListener("change", () => this.UploadFiles(Array.from(this.FileInput.files ?? []), f => f.name));
        this.FolderInput.addEventListener("change", () => this.UploadFolder());
        this.SkillSelect.addEventListener("change", () => {
            this.Skill = this.SkillSelect.value || null;
            localStorage.setItem("AgentSkill", this.Skill ?? "");
        });
        runsBtn.addEventListener("click", () => this.OpenRuns());

        this.append(PanelStyle, html`<div>
             <div class="acp-header">Contexto del agente</div>
            <label class="acp-label">Carpeta de trabajo</label>
            ${this.RootLabel}
            <div class="acp-row">${this.PathInput}${usePathBtn}</div>
            ${pickFolderBtn}
            <label class="acp-label">Skill</label>
            ${this.SkillSelect}
            <div class="acp-block-title"><span>Usuario (solo lectura)</span>${uploadBtn}</div>
            ${this.UserBlock}
            <div class="acp-block-title"><span>Gestionados por el agente</span></div>
            ${this.AgentBlock}
            ${runsBtn}
            ${this.Status}
            ${this.FileInput}
            ${this.FolderInput}
        </div>`);
        this.Init();
    }

    Init = async () => {
        try {
            const skills = await AgentGet(`${this.ApiBase}/AgentSkills`);
            this.SkillSelect.append(new Option("(general)", ""));
            skills.forEach(s => this.SkillSelect.append(new Option(s.Name, s.Name, false, s.Name === this.Skill)));
            await this.Refresh();
        } catch (e) { this.SetStatus(e.message, true); }
    }

    Refresh = async () => {
        const dto = await AgentGet(`${this.ApiBase}/AgentFiles/list?token=${encodeURIComponent(this.Token)}&userId=${encodeURIComponent(this.UserId)}`);
        this.RenderBlocks(dto);
    }

    UsePath = async () => {
        const path = this.PathInput.value.trim();
        if (!path) { this.SetStatus("Escribe la ruta de la carpeta.", true); return; }
        if (this.RootPath && this.RootPath !== path &&
            !confirm("Cambiar la carpeta reinicia el contexto de archivos de este chat. ¿Continuar?")) return;
        try {
            const dto = await AgentPost(`${this.ApiBase}/AgentWorkspace/setFolder`, {
                Token: this.Token, UserId: this.UserId, Path: path
            });
            this.RenderBlocks(dto);
            this.SetStatus(`Carpeta vinculada: ${dto.UserFiles?.length ?? 0} archivos registrados como contexto.`);
        } catch (e) { this.SetStatus(e.message ?? "Error al vincular la carpeta", true); }
    }

    UploadFolder = async () => {
        const all = Array.from(this.FolderInput.files ?? []);
        const files = all.filter(f => {
            const rel = (f.webkitRelativePath || f.name).toLowerCase();
            if (rel.split("/").some(seg => IGNORED_FOLDERS.includes(seg))) return false;
            if (!ALLOWED_EXT.some(ext => rel.endsWith(ext))) return false;
            return f.size <= MAX_UPLOAD_BYTES;
        }).slice(0, MAX_FOLDER_FILES);
        if (files.length === 0) { this.SetStatus("La carpeta no tiene archivos de texto/código permitidos.", true); return; }
        if (all.length > files.length) this.SetStatus(`Se subirán ${files.length} de ${all.length} (filtrados por tipo/tamaño/límite).`);
        await this.UploadFiles(files, f => f.webkitRelativePath || f.name);
        this.FolderInput.value = "";
    }

    UploadFiles = async (files, nameOf) => {
        let ok = 0, fail = 0;
        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            if (file.size > MAX_UPLOAD_BYTES) { fail++; continue; }
            this.SetStatus(`Subiendo ${i + 1}/${files.length}: ${nameOf(file)}`);
            try {
                await AgentPost(`${this.ApiBase}/AgentFiles/upload`, {
                    Token: this.Token, UserId: this.UserId,
                    Attach: { Name: nameOf(file), Type: file.type, Value: await ReadAsBase64(file) }
                });
                ok++;
            } catch (e) { fail++; console.log(nameOf(file), e.message); }
        }
        await this.Refresh().catch(console.log);
        this.SetStatus(`${ok} archivos cargados${fail ? `, ${fail} rechazados (tipo/tamaño)` : ""}.`, fail > 0 && ok === 0);
        this.FileInput.value = "";
    }

    SetExcluded = async (idFile, excluded) => {
        try {
            const dto = await AgentPost(`${this.ApiBase}/AgentFiles/exclude`, {
                Token: this.Token, UserId: this.UserId, Id_File: idFile, Excluded: excluded
            });
            this.RenderBlocks(dto);
        } catch (e) { this.SetStatus(e.message ?? "Error al excluir", true); }
    }

    RenderBlocks(dto) {
        this.WorkspaceId = dto.Id_Workspace;
        this.RootPath = dto.RootPath;
        this.RootLabel.textContent = dto.RootPath ?? "(sin carpeta elegida)";
        this.RootLabel.title = dto.RootPath ?? "";
        this.UserBlock.innerHTML = "";
        this.AgentBlock.innerHTML = "";
        const render = (files, container) => {
            if (!files?.length) { container.append(html`<div class="acp-empty">(ninguno)</div>`); return; }
            files.forEach(f => {
                const check = WRender.Create({ tagName: "input", type: "checkbox", title: "Incluir en el contexto" });
                check.checked = !f.Excluded;
                check.addEventListener("change", () => this.SetExcluded(f.Id_File, !check.checked));
                container.append(html`<div class="acp-file ${f.Excluded ? "excluded" : ""}">
                    ${check}
                    <span class="acp-name" title="${f.RelativePath}">${f.RelativePath}</span>
                    <span class="acp-size">${Math.ceil((f.SizeBytes ?? 0) / 1024)} KB</span>
                </div>`);
            });
        };
        render(dto.UserFiles, this.UserBlock);
        render(dto.AgentFiles, this.AgentBlock);
    }

    OpenRuns = async () => {
        try {
            const runs = await AgentGet(`${this.ApiBase}/AgentRun/byToken?token=${encodeURIComponent(this.Token)}&userId=${encodeURIComponent(this.UserId)}`);
            document.body.append(new WModalForm({
                ObjectModal: html`<div style="min-width:420px; padding:12px;">
                    <h3>Ejecuciones de esta conversación</h3>
                    ${runs.map(r => html`<div style="display:flex; justify-content:space-between; gap:8px; padding:6px 0; border-bottom:1px solid #ddd;">
                        <span>#${r.Id_Run} · ${r.Skill} · ${r.Status} · ${r.Steps} pasos</span>
                        <button class="acp-btn" onclick="${() => OpenRunTraceModal(r.Id_Run, this.UserId, this.ApiBase)}">Ver pasos</button>
                    </div>`)}
                </div>`
            }));
        } catch (e) { this.SetStatus(e.message ?? "Error al cargar ejecuciones", true); }
    }

    SetAgentMode(on) { this.classList.toggle("acp-disabled", !on); }
    SetStatus(msg, isError = false) {
        this.Status.textContent = msg;
        this.Status.classList.toggle("error", isError);
    }
}

const PanelStyle = css`
    .agent-context-panel { display: flex; flex-direction: column; gap: 6px; padding: 10px; overflow-y: auto;
        border-left: 1px solid var(--primary-border, #ccc); background: var(--secundary-color, #f7f7f7); font-size: 13px; }
    .agent-context-panel.acp-disabled { opacity: .45; pointer-events: none; }
    .acp-header { font-weight: 600; font-size: 14px; }
    .acp-label { font-size: 11px; opacity: .6; text-transform: uppercase; margin-top: 6px; }
    .acp-root { font-size: 11px; padding: 4px 6px; background: var(--primary-color, #eee); border-radius: 6px;
        overflow: hidden; text-overflow: ellipsis; white-space: nowrap; direction: rtl; text-align: left; }
    .acp-select, .acp-input { width: 100%; padding: 6px; border-radius: 6px; border: 1px solid var(--primary-border, #ccc); }
    .acp-row { display: grid; grid-template-columns: 1fr auto; gap: 6px; }
    .acp-btn { padding: 6px 10px; border: none; border-radius: 6px; cursor: pointer;
        background: var(--primary-color, #e3e3e3); color: var(--font-primary-color, #222); }
    .acp-btn:hover { background: var(--fourth-color, #d5d5d5); }
    .acp-wide { width: 100%; }
    .acp-block-title { display: flex; justify-content: space-between; align-items: center; font-weight: 600; margin-top: 10px; }
    .acp-block { display: flex; flex-direction: column; gap: 2px; }
    .files { max-height: 300px; overflow: auto}
    .acp-file { display: grid; grid-template-columns: auto 1fr auto; gap: 6px; align-items: center; padding: 4px 2px; }
    .acp-file.excluded .acp-name { text-decoration: line-through; opacity: .5; }
    .acp-name { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .acp-size { font-size: 11px; opacity: .6; }
    .acp-empty { opacity: .5; font-style: italic; padding: 4px; }
    .acp-status { font-size: 12px; min-height: 16px; opacity: .8; }
    .acp-status.error { color: #c0392b; }
`;

customElements.define("w-agent-context-panel", WAgentContextPanel);