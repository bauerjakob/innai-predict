import type { HistoryResultDto } from "../dtos/HistoryResultDto";

const { PREDICT_SERVER_BASE_URL } = import.meta.env;

export class PredictService {

    async predictCurrent(): Promise<number[]> {
        const request = this.GetRequestInfo("innAi/predict/current");
        return fetch(request)
            .then(res => res.json())
            .then(res => {
                return res as number[]
            });
    }

    async predictDate(isoDate: string): Promise<HistoryResultDto> {
        const date = new Date(isoDate);

        return this.predictDate2(date.getFullYear(), date.getMonth() + 1, date.getDate());
    }

    async predictDate2(year: number, month: number, day: number): Promise<HistoryResultDto> {
        const request = this.GetRequestInfo("innAi/predict/history?year=" + year + "&month=" + month + "&day=" + day + "&hour=0");
        return fetch(request)
            .then(res => res.json())
            .then(res => {
                return res as HistoryResultDto
            });
    }

    async actualDate(isoDate: string): Promise<number[]> {
        const date = new Date(isoDate);

        return this.actualDate2(date.getFullYear(), date.getMonth() + 1, date.getDate());
    }

    async actualDate2(year: number, month: number, day: number): Promise<number[]> {
        const request = this.GetRequestInfo("innAi/actual?year=" + year + "&month=" + month + "&day=" + day + "&hour=0");
        return fetch(request)
            .then(res => res.json())
            .then(res => {
                return res as number[]
            });
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