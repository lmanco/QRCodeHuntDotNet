import fs from 'fs';

import qrcode from 'qrcode';
import jimp from 'jimp';
import * as uuid from 'uuid';

import DataWriter from './Util/DataWriter.js';
import DataReader from './Util/DataReader.js';
import CodeRepository from './CodeRepository.js';
import CodeGenerator from './CodeGenerator.js';

const args = process.argv.slice(2);
const config = { dataDir: args[0], baseUrl: args[1], huntConfig: { name: args[2], numCodes: args[3] } };
const dataRoot = `${config.dataDir}/${config.huntConfig.name}`;
const dataWriter = new DataWriter(fs, dataRoot, console);
const dataReader = new DataReader(fs, dataRoot, console);
const codeRepository = new CodeRepository(dataWriter, dataReader, dataRoot, config.baseUrl, fs, qrcode, jimp);

new CodeGenerator(config, fs, console, codeRepository, uuid).run();