import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigureFeatureComponent } from './configure-feature.component';

describe('ConfigureFeatureComponent', () => {
  let component: ConfigureFeatureComponent;
  let fixture: ComponentFixture<ConfigureFeatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigureFeatureComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfigureFeatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
