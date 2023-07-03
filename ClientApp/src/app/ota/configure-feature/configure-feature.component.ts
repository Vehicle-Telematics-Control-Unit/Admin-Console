import { Component, OnInit } from '@angular/core';
import { OTADataService } from '../ota-data.service';

@Component({
  selector: 'app-configure-feature',
  templateUrl: './configure-feature.component.html',
  styleUrls: ['./configure-feature.component.css']
})
export class ConfigureFeatureComponent implements OnInit {
  modifyFeatureFormInfo: any = {
    FeatureId: null,
    appName: null,
    releaseId: null,
    releaseTime: null,
    description: null,
    modelId: null,
    env_vars: null,
    ports: null,
    volumes: null
  }

  featuresInfo!: any[];
  releasesInfo!: any[];
  modelsInfo!: any[];
  constructor(private _OTA_Data: OTADataService) { }

  ngOnInit(): void {
    this._OTA_Data.getFeatures().subscribe((res: any) => {
      this.featuresInfo = res.features;
    });

    this._OTA_Data.getAppReleases().subscribe((res: any) => {
      this.releasesInfo = res.apps
    });

    this._OTA_Data.getTcuModels().subscribe((res: any) => {
      this.modelsInfo = res.models;
    });
  }

  updateConfig(selection: any) {
    let selectionId: number = selection.id;
    let selectionName: string = selection.name;
    this.modifyFeatureFormInfo.appName = selectionName;
    this._OTA_Data.getFeatureInfo(selectionId).subscribe((res:any) => {
      this.releasesInfo.push(res.release);
      this.modifyFeatureFormInfo.FeatureId = selectionId;
      this.modifyFeatureFormInfo.releaseId = res.release.id;
      this.modifyFeatureFormInfo.releaseTime = res.releaseDate;
      this.modifyFeatureFormInfo.description = res.description;
      this.modifyFeatureFormInfo.modelId = res.distributionModels;
      this.modifyFeatureFormInfo.env_vars = (res.env_Variables as string[]).join("\n");
      this.modifyFeatureFormInfo.ports = (res.portsBinding as string[]).join("\n");
      this.modifyFeatureFormInfo.volumes = (res.volumesBinding as string[]).join("\n");
    });
  }

  onSubmit() {
    this._OTA_Data.modifyfeature(this.modifyFeatureFormInfo).subscribe(res => {
      alert("Feature modified successfully");
      let featureId = this.modifyFeatureFormInfo.FeatureId
      this.updateConfig(featureId);
    });

  }

}
