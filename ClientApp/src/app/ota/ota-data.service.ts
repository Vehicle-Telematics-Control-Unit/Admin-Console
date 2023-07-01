import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class OTADataService {
  private headers = {
    'Content-Type': 'application/json;charset=utf-8;',
    'lang': localStorage.getItem('language') === 'en' ? 'en' : 'ar'
  }

  constructor(private _httpClient: HttpClient) { }

  public getTcuModels() {
    return this._httpClient.get('/api/OTA/getModels/', {
      headers: this.headers
    });
  }

  public getAppReleases() {
    return this._httpClient.get('/api/OTA/getApps/', {
      headers: this.headers
    });
  }


  public publishRelease(addFeatureFormInfo: any) {
    return this._httpClient.post('/api/OTA/publishFeature/', addFeatureFormInfo, {
      headers: this.headers
    });
  }
}
