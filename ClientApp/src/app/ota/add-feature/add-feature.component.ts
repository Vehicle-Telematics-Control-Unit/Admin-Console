import { Component, OnInit, ViewChild } from '@angular/core';
import { OTADataService } from '../ota-data.service';

@Component({
  selector: 'app-add-feature',
  templateUrl: './add-feature.component.html',
  styleUrls: ['./add-feature.component.css']
})
export class AddFeatureComponent implements OnInit {

  constructor(private _OTA_Data: OTADataService) { }
  modelsInfo!: any[];
  releasesInfo!: any[];
  addFeatureFormInfo: any = {
    appName: null,
    modelId: null,
    releaseId: null,
    releaseTime: null,
    description: null
  }  
  ngOnInit(): void {
    this._OTA_Data.getTcuModels().subscribe((res: any) => {
      this.modelsInfo = res.models;
    });

    this._OTA_Data.getAppReleases().subscribe((res: any) => {
      this.releasesInfo = res.apps
    });
  }

  onSubmit() {
    this._OTA_Data.publishRelease(this.addFeatureFormInfo).subscribe(res => {
      alert("Feature published successfully");
    });
  }
}
