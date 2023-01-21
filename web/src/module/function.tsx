import * as React from 'react';
import {URI } from '@opensumi/ide-core-browser';

import { WorkbenchEditorService } from '@opensumi/ide-editor/lib/browser';
import { promisify } from '@opensumi/ide-core-common/lib/browser-fs/util';
import * as fs from 'fs';
import { FileUri } from '@opensumi/ide-core-common';
import CompileInteraction from './interaction/compile.interaction'
import { Button } from '@opensumi/ide-components';

const COMPONENTS_SCHEME_ID = 'antd-theme';

interface IProps {
  work: WorkbenchEditorService;
  editorService:WorkbenchEditorService
}

interface IState {

}

export default class FunctionView extends React.Component<IProps, IState> {

  constructor(props) {
    super(props)
  }

  async RazorRender() {
    this.props.editorService.open(new URI(`${COMPONENTS_SCHEME_ID}://`),
    { preview: false, focus: false, label: '渲染区', split: 4 });
    var content = await promisify(fs.readFile)(FileUri.fsPath(this.props.work.getAllOpenedUris()[0]), { encoding: 'utf8' });
    CompileInteraction.CompileRazor(content)
  }

  render(): React.ReactNode {
    return (<>
      <div style={{ margin: '5px' }}><Button block={true} onClick={() => this.RazorRender()} >渲染</Button></div>
      </>)
  }
}

