import type { HistoryResultDto } from "../dtos/HistoryResultDto";
import type { AiModelDto } from "../dtos/AiModelDto";

const { PREDICT_SERVER_BASE_URL } = import.meta.env;

export class PredictService {

    async predictCurrent(modelId: string | null): Promise<number[]> {
        const request = this.GetRequestInfo("innAi/predict/current" + this.getModelIdAppendex(modelId, "?"));
        return fetch(request)
            .then(res => res.json())
            .then(res => {
                return res as number[]
            });
    }

    async predictDate(modelId: string | null, isoDate: string): Promise<HistoryResultDto> {
        const date = new Date(isoDate);

        return this.predictDate2(modelId, date.getFullYear(), date.getMonth() + 1, date.getDate());
    }

    async predictDate2(modelId: string | null, year: number, month: number, day: number): Promise<HistoryResultDto> {
        const request = this.GetRequestInfo("innAi/predict/history?year=" + year + "&month=" + month + "&day=" + day + "&hour=0" + this.getModelIdAppendex(modelId, "&"));
        return fetch(request)
            .then(res => res.json())
            .then(res => {
                return res as HistoryResultDto
            });
    }

    async getAiModelsAsync(): Promise<AiModelDto[]> {
        const request = this.GetRequestInfo("innAi/models");
        return fetch(request)
            .then(res => res.json())
            .then(res => {
                return res as AiModelDto[]
            });
    }

    private getModelIdAppendex(modelId: string | null, leading: string) {
        return modelId ? leading + "modelId=" + modelId : "";
    }

    private GetRequestInfo(route: string) {
        const requestUrl = `${PREDICT_SERVER_BASE_URL}/${route}`;
        const headers: Headers = new Headers()
        headers.set('Content-Type', 'application/json')
        headers.set('Accept', 'application/json')

        const requestInfo: RequestInfo = new Request(requestUrl, {
            method: 'GET',
            headers: headers
        })

        return requestInfo;
    }
}